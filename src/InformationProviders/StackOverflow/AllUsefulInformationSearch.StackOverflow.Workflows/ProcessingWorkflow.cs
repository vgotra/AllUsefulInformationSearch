namespace AllUsefulInformationSearch.StackOverflow.Workflows;

public class StackOverflowProcessingWorkflow(ILogger<StackOverflowProcessingWorkflow> logger) : IWorkflow
{
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting StackOverflow processing workflow");
        // Step 1: Sync web data files to db
        // Step 2: Get updated/new files list from db
        // Step 3: Download/extract/deserialize/cleanup files (to minimize disk space usage)
        // Step 4: Process deserialized data in parallel (cleanup text, etc.)
        // Step 5: Save processed data to db
        // Step 6: Post processing of data, etc
        // Step 7: Summary
        logger.LogInformation("Completed StackOverflow processing workflow");
        await Task.CompletedTask;
    }
}