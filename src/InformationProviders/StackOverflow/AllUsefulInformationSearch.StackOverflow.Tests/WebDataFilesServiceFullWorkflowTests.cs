﻿namespace AllUsefulInformationSearch.StackOverflow.Tests;

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
        
        var cancellationTokenSource = new CancellationTokenSource();
        var dbContextOptions = new DbContextOptionsBuilder<StackOverflowDbContext>()
            .UseSqlServer("Server=.;Database=AllUsefulInformationSearch_StackOverflow;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;").Options;
        var dbContext = new StackOverflowDbContext(dbContextOptions);
        var logger = new TestContextLogger<WebArchiveParser>(TestContext);
        var parser = new WebArchiveParser(logger);
        var service = new WebDataFilesService(dbContext, parser);
        await service.SynchronizeWebDataFilesAsync(cancellationTokenSource.Token);
        var itemsCount = await dbContext.WebDataFiles.CountAsync(cancellationTokenSource.Token);
        Assert.IsTrue(itemsCount > 0);

        var httpClient = new HttpClient { BaseAddress = new Uri("https://archive.org/download/stackexchange/") };
        IFileUtilityService fileUtilityService = Environment.OSVersion.Platform == PlatformID.Win32NT ? new WindowsFileUtilityService(httpClient) : new LinuxFileUtilityService(httpClient); // add MacOS later
        var webArchiveFileService = new WebArchiveFileService(fileUtilityService);
        var files = await dbContext.WebDataFiles.AsNoTracking().Where(x => x.Size < 10 * FileSize.Mb).Take(countOfFilesToProcess).ToListAsync(cancellationTokenSource.Token);
        Assert.IsTrue(files.Count == countOfFilesToProcess);

        await Parallel.ForEachAsync(files, cancellationTokenSource.Token, async (webDataFile, token) =>
        {
            TestContext.WriteLine($"Started processing file {webDataFile.Name}");
            
            await using var dbContextInstance = new StackOverflowDbContext(dbContextOptions);
            
            var fileFromDb = await dbContextInstance.WebDataFiles.FirstAsync(x => x.Id == webDataFile.Id, token);
            fileFromDb.ProcessingStatus = ProcessingStatus.InProgress;
            await dbContextInstance.SaveChangesAsync(token);
            
            var paths = new WebFilePaths { WebFileUri = webDataFile.Link, TemporaryDownloadPath = Path.GetTempFileName(), ArchiveOutputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()) };
            var posts = await webArchiveFileService.GetPostsWithCommentsAsync(paths, token);

            var entities = posts.Select(x => x.ToEntity(webDataFile.Id));
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