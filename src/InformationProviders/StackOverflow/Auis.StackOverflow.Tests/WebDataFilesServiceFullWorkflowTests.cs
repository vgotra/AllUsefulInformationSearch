namespace Auis.StackOverflow.Tests;

[TestClass]
public class WebDataFilesServiceFullWorkflowTests : BaseTests
{
    //TODO Find better solution for ignoring tests 
    [TestMethod]
#if !DEBUG
    [Ignore("This test is for manual execution only")]
#endif
    public async Task CanProcessFirstFiles()
    {
        const int countOfFilesToProcess = 5;
        var httpClient = new HttpClient { BaseAddress = new Uri("https://archive.org/download/stackexchange/") };
        
        var cancellationTokenSource = new CancellationTokenSource();
        var dbContextOptions = new DbContextOptionsBuilder<StackOverflowDbContext>()
            .UseSqlServer("Server=.;Database=Auis_StackOverflow;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;").Options;
        var dbContext = new StackOverflowDbContext(dbContextOptions);
        var parser = new WebArchiveParserService(httpClient, new TestContextLogger<WebArchiveParserService>(TestContext));
        var service = new WebDataFilesService(dbContext, parser, new TestContextLogger<WebDataFilesService>(TestContext));
        await service.SynchronizeWebDataFilesAsync(cancellationTokenSource.Token);
        var itemsCount = await dbContext.WebDataFiles.CountAsync(cancellationTokenSource.Token);
        Assert.IsTrue(itemsCount > 0);
        
        IFileUtilityService fileUtilityService = Environment.OSVersion.Platform == PlatformID.Win32NT ? new WindowsFileUtilityService(httpClient) : new LinuxFileUtilityService(httpClient); // add MacOS later
        var webArchiveFileService = new WebArchiveFileService(fileUtilityService, new TestContextLogger<WebArchiveFileService>(TestContext));
        var files = await dbContext.WebDataFiles.AsNoTracking().Where(x => x.Size < 10 * FileSize.Mb).Take(countOfFilesToProcess).ToListAsync(cancellationTokenSource.Token);
        Assert.IsTrue(files.Count == countOfFilesToProcess);

        await Parallel.ForEachAsync(files, cancellationTokenSource.Token, async (webDataFile, token) =>
        {
            TestContext.WriteLine($"Started processing file {webDataFile.Name}");
            
            await using var dbContextInstance = new StackOverflowDbContext(dbContextOptions);
            
            var fileFromDb = await dbContextInstance.WebDataFiles.FirstAsync(x => x.Id == webDataFile.Id, token);
            fileFromDb.ProcessingStatus = ProcessingStatus.InProgress;
            await dbContextInstance.SaveChangesAsync(token);
            
            var paths = webDataFile.ToWebFilePaths();
            var posts = await webArchiveFileService.GetPostsWithCommentsAsync(paths, token);

            var entities = posts.Select(x => x.ToEntity());
            await dbContextInstance.Posts.AddRangeAsync(entities, token);
            await dbContextInstance.SaveChangesAsync(token);
            
            fileFromDb.ProcessingStatus = ProcessingStatus.Processed;
            await dbContextInstance.SaveChangesAsync(token);
            
            TestContext.WriteLine($"Completed processing file {webDataFile.Name}");
        });
        
        var processedFilesCount = await dbContext.WebDataFiles.AsNoTracking().Where(x => x.ProcessingStatus == ProcessingStatus.Processed).CountAsync(cancellationTokenSource.Token);
        Assert.IsTrue(processedFilesCount == countOfFilesToProcess);
    }
}