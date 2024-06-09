namespace Auis.StackOverflow.Workflows;

public class StackOverflowProcessingSubWorkflow(IServiceProvider serviceProvider, ILogger<StackOverflowProcessingSubWorkflow> logger) : IStackOverflowProcessingSubWorkflow
{
    private static readonly ActivitySource TracingSource = new("StackOverflowProcessingSubWorkflow");

    public async Task ExecuteAsync(WebDataFileEntity webDataFileEntity, CancellationToken cancellationToken = default) =>
        await TracingSource.ExecuteWithTracingAsync("ExecuteAsync", async (_, token) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<StackOverflowOptions>>();
            var webFilePaths = webDataFileEntity.ToWebFilePaths(options.Value);

            logger.LogInformation("Starting StackOverflow processing sub workflow for {WebFileUri}", webFilePaths.FileUri);

            var posts = await serviceProvider.GetRequiredService<IArchiveFileService>().GetPostsWithCommentsAsync(webFilePaths, token);
            var modifiedPosts = await serviceProvider.GetRequiredService<IPostModificationService>().PostProcessArchivePostsAsync(posts, token);
            await serviceProvider.GetRequiredService<IPostsSynchronizationService>().SynchronizePostsAsync(webFilePaths, modifiedPosts, token);
            await serviceProvider.GetRequiredService<IWebDataFilesRepository>().SetProcessingStatusAsync(webDataFileEntity, ProcessingStatus.Processed, token);

            logger.LogInformation("Completed StackOverflow processing sub workflow for {WebFileUri}", webFilePaths.FileUri);
        }, cancellationToken);
}