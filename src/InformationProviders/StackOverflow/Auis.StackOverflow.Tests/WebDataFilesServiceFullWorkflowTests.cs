using Auis.StackOverflow.App;
using Auis.StackOverflow.Workflows;

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

        var cancellationTokenSource = new CancellationTokenSource();

        var service = host.Services.GetRequiredService<IWebDataFilesService>();
        await service.SynchronizeWebDataFilesAsync(cancellationTokenSource.Token);
        var itemsCount = await dbContext.WebDataFiles.CountAsync(cancellationTokenSource.Token);
        Assert.IsTrue(itemsCount > 0);

        var webArchiveFileService = host.Services.GetRequiredService<IWebArchiveFileService>();
        var files = await dbContext.WebDataFiles.AsNoTracking().Where(x => x.Size < 10 * FileSize.Mb).Take(countOfFilesToProcess).ToListAsync(cancellationTokenSource.Token);
        Assert.IsTrue(files.Count == countOfFilesToProcess);

        await Parallel.ForEachAsync(files, cancellationTokenSource.Token, async (webDataFile, token) => await host.Services.GetRequiredService<IStackOverflowProcessingSubWorkflow>().ExecuteAsync(webDataFile, cancellationTokenSource.Token));

        var processedFilesCount = await dbContext.WebDataFiles.AsNoTracking().Where(x => x.ProcessingStatus == ProcessingStatus.Processed).CountAsync(cancellationTokenSource.Token);
        Assert.IsTrue(processedFilesCount == countOfFilesToProcess);
    }
}