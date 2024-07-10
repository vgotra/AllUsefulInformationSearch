using Auis.Wikipedia.BusinessLogic.Helpers;

namespace Auis.Wikipedia.BusinessLogic.Workflows;

public class WikipediaProcessingSubWorkflow(IOptions<WikipediaOptions> options, IPostsArchiveFileProcessingService postsArchiveFileProcessingService, ILogger<WikipediaProcessingSubWorkflow> logger)
    : IWikipediaProcessingSubWorkflow
{
    public async Task ExecuteAsync(WebDataFileEntity webDataFileEntity, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting Wikipedia processing sub workflow for {WebFileName}", webDataFileEntity.Name);
        if (options.Value.UseSubProcessForProcessingFile)
        {
            var processDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName);
            if (processDirectory == null)
                throw new InvalidOperationException("Could not get process directory.");
            var pathToExecute = Path.Combine(processDirectory, options.Value.SubProcessFileName);
            if (!Path.Exists(pathToExecute))
                throw new InvalidOperationException($"Sub process file: {pathToExecute} does not exist.");
            await pathToExecute.ExecuteProcessAsync($" {webDataFileEntity.Name}", cancellationToken);
            //TODO Check for errors
        }
        else
        {
            await postsArchiveFileProcessingService.ProcessArchiveFileAsync(webDataFileEntity, cancellationToken);
        }

        logger.LogInformation("Completed Wikipedia processing sub workflow for {WebFileName}", webDataFileEntity.Name);
    }
}