namespace Auis.StackOverflow.BusinessLogic.Workflows;

public class StackOverflowProcessingWorkflow(
    IWebArchivesSynchronizationService webArchivesSynchronizationService,
    IDbContextFactory<StackOverflowDbContext> dbContextFactory,
    IStackOverflowProcessingSubWorkflow stackOverflowProcessingSubWorkflow,
    ILogger<StackOverflowProcessingWorkflow> logger) : IStackOverflowProcessingWorkflow
{
    //TODO Refactor later, SOLID, tracking or errors for every web files, etc.
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Starting StackOverflow processing workflow");

            await webArchivesSynchronizationService.SynchronizeWebArchiveFiles(cancellationToken);

            await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var webFiles = await dbContext.WebDataFiles
                .Where(x => x.IsSynchronizationEnabled && (x.ProcessingStatus == ProcessingStatus.Updated || x.ProcessingStatus == ProcessingStatus.New))
                .ToListAsync(cancellationToken);

            var parallelOptions = new ParallelOptions { CancellationToken = cancellationToken, MaxDegreeOfParallelism = Environment.ProcessorCount == 1 ? 1 : Environment.ProcessorCount / 2 };
            await Parallel.ForEachAsync(webFiles, parallelOptions, async (webFile, token) =>
            {
                try
                {
                    await stackOverflowProcessingSubWorkflow.ExecuteAsync(webFile, token);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error during StackOverflow processing sub workflow for {WebFileName}", webFile.Name);
                }
            });

            //TODO Maybe some summary to db or similar, etc.

            logger.LogInformation("Completed StackOverflow processing workflow");
        }
        catch (Exception e)
        {
            //TODO Update web files processing status to failed status
            logger.LogCritical(e, "Error during StackOverflow processing workflow");
            throw;
        }
    }
}