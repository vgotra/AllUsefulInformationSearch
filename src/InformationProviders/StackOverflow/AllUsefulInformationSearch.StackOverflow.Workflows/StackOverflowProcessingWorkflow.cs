﻿namespace AllUsefulInformationSearch.StackOverflow.Workflows;

public class StackOverflowProcessingWorkflow(IServiceProvider serviceProvider, ILogger<StackOverflowProcessingWorkflow> logger) : IStackOverflowProcessingWorkflow
{
    //TODO Refactor later, SOLID, tracking or errors for every web files, etc.
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Starting StackOverflow processing workflow");
           
            await serviceProvider.GetRequiredService<IWebDataFilesService>().SynchronizeWebDataFilesAsync(cancellationToken);

            var webFilesRepository = serviceProvider.GetRequiredService<IWebDataFilesRepository>();
            var webFiles = await webFilesRepository.GetWebDataFilesAsync(cancellationToken);

            await Parallel.ForEachAsync(webFiles, cancellationToken, async (webFile, token) =>
            {
                var subWorkflow = serviceProvider.GetRequiredService<IStackOverflowProcessingSubWorkflow>();
                await subWorkflow.ExecuteAsync(webFile, token);
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