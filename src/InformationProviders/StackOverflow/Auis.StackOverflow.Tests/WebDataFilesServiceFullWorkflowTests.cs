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
        var host = Host.CreateDefaultBuilder().ConfigureServices((context, services) => services.ConfigureServices(context)).Build();

        const int countOfFilesToProcess = 5;
        var dbContext = host.Services.GetRequiredService<StackOverflowDbContext>();
        //TODO Reuse in memory db or clean db before each test and apply migrations

        var cancellationTokenSource = new CancellationTokenSource();

        var mediator = host.Services.GetRequiredService<IMediator>();
        await mediator.Send(new RefreshWebArchiveFilesRequest(), cancellationTokenSource.Token);

        var itemsCount = await dbContext.WebDataFiles.CountAsync(cancellationTokenSource.Token);
        Assert.IsTrue(itemsCount > 0);

        var files = await dbContext.WebDataFiles.AsNoTracking().Where(x => x.Size < 10 * FileSize.Mb).OrderBy(x => x.Size).Take(countOfFilesToProcess).ToListAsync(cancellationTokenSource.Token);
        Assert.IsTrue(files.Count == countOfFilesToProcess);

        await Parallel.ForEachAsync(files, cancellationTokenSource.Token, async (webDataFile, token) => await host.Services.GetRequiredService<IStackOverflowProcessingSubWorkflow>().ExecuteAsync(webDataFile, cancellationTokenSource.Token));

        var processedFilesCount = await dbContext.WebDataFiles.AsNoTracking().Where(x => x.ProcessingStatus == ProcessingStatus.Processed).CountAsync(cancellationTokenSource.Token);
        Assert.IsTrue(processedFilesCount == countOfFilesToProcess);
    }
}