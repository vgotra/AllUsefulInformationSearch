using Auis.StackOverflow.BusinessLogic.Helpers;
using Auis.StackOverflow.Common.Options;

namespace Auis.StackOverflow.BusinessLogic.Workflows;

public class StackOverflowProcessingSubWorkflow(IOptions<StackOverflowOptions> options, IPostsArchiveFileProcessingService postsArchiveFileProcessingService, ILogger<StackOverflowProcessingSubWorkflow> logger)
    : IStackOverflowProcessingSubWorkflow
{
    public async Task ExecuteAsync(WebDataFileEntity webDataFileEntity, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting StackOverflow processing sub workflow for {WebFileName}", webDataFileEntity.Name);
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

        logger.LogInformation("Completed StackOverflow processing sub workflow for {WebFileName}", webDataFileEntity.Name);
    }
}