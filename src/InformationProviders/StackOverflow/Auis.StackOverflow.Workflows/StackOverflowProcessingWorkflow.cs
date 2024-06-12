using Auis.StackOverflow.Services.Handlers;

namespace Auis.StackOverflow.Workflows;

public class StackOverflowProcessingWorkflow(IServiceProvider serviceProvider, IMediator mediator, ILogger<StackOverflowProcessingWorkflow> logger) : IStackOverflowProcessingWorkflow
{
    //TODO Refactor later, SOLID, tracking or errors for every web files, etc.
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Starting StackOverflow processing workflow");

            _ = await mediator.Send(new RefreshWebArchiveFilesRequest(), cancellationToken);

            var webFilesRepository = serviceProvider.GetRequiredService<IWebDataFilesRepository>();
            var webFiles = await webFilesRepository.GetWebDataFilesAsync(cancellationToken);

            var parallelOptions = new ParallelOptions { CancellationToken = cancellationToken, MaxDegreeOfParallelism = Environment.ProcessorCount == 1 ? 1 : Environment.ProcessorCount / 2};
            await Parallel.ForEachAsync(webFiles, parallelOptions, async (webFile, token) =>
            {
                try
                {
                    await mediator.Send(new PostsProcessingCommand(webFile), token);
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