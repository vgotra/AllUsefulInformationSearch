namespace Auis.Wikipedia.BusinessLogic.Workflows;

public class WikipediaProcessingWorkflow(
    IWebArchivesSynchronizationService webArchivesSynchronizationService,
    IDbContextFactory<WikipediaDbContext> dbContextFactory,
    IWikipediaProcessingSubWorkflow stackOverflowProcessingSubWorkflow,
    ILogger<WikipediaProcessingWorkflow> logger) : IWikipediaProcessingWorkflow
{
    //TODO Refactor later, SOLID, tracking or errors for every web files, etc.
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Starting Wikipedia processing workflow");

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
                    logger.LogError(e, "Error during Wikipedia processing sub workflow for {WebFileName}", webFile.Name);
                }
            });

            //TODO Maybe some summary to db or similar, etc.

            logger.LogInformation("Completed Wikipedia processing workflow");
        }
        catch (Exception e)
        {
            //TODO Update web files processing status to failed status
            logger.LogCritical(e, "Error during Wikipedia processing workflow");
            throw;
        }
    }
}