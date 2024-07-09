namespace Auis.Wikipedia.Tests;

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
        var cancellationTokenSource = new CancellationTokenSource();

        var host = Host.CreateDefaultBuilder().ConfigureServices((context, services) => services.ConfigureServices(context)).Build();

        const int countOfFilesToProcess = 1;
        await using var dbContext = await host.Services.GetRequiredService<IDbContextFactory<WikipediaDbContext>>().CreateDbContextAsync(cancellationTokenSource.Token);

        var webArchivesSynchronizationService = host.Services.GetRequiredService<IWebArchivesSynchronizationService>();
        await webArchivesSynchronizationService.SynchronizeWebArchiveFiles(cancellationTokenSource.Token);

        var itemsCount = await dbContext.WebDataFiles.CountAsync(cancellationTokenSource.Token);
        Assert.IsTrue(itemsCount > 0);

        var files = await dbContext.WebDataFiles.AsNoTracking().Where(x => x.Size < 10 * FileSize.Mb).OrderBy(x => x.Size).Take(countOfFilesToProcess).ToListAsync(cancellationTokenSource.Token);
        Assert.IsTrue(files.Count == countOfFilesToProcess);

        var postsArchiveFileProcessingService = host.Services.GetRequiredService<PostsArchiveFileProcessingServiceMock>();
        await Parallel.ForEachAsync(files, cancellationTokenSource.Token, async (webDataFile, token) => await postsArchiveFileProcessingService.ProcessArchiveFileAsync(webDataFile, token));

        var processedFilesCount = await dbContext.WebDataFiles.AsNoTracking().Where(x => x.ProcessingStatus == ProcessingStatus.Processed).CountAsync(cancellationTokenSource.Token);
        Assert.IsTrue(processedFilesCount == countOfFilesToProcess);
    }
}