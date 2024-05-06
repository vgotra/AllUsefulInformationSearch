namespace AllUsefulInformationSearch.StackOverflow.Workflows;

public class StackOverflowProcessingSubWorkflow(IServiceProvider serviceProvider, ILogger<StackOverflowProcessingSubWorkflow> logger) : IStackOverflowProcessingSubWorkflow
{
    public async Task ExecuteAsync(WebDataFileEntity webDataFileEntity, CancellationToken cancellationToken = default)
    {
        var webFilePaths = webDataFileEntity.ToWebFilePaths();
        
        logger.LogInformation("Starting StackOverflow processing sub workflow for {WebFileUri}", webFilePaths.WebFileUri);
        
        var posts = await serviceProvider.GetRequiredService<IWebArchiveFileService>().GetPostsWithCommentsAsync(webFilePaths, cancellationToken);
        
        var modifiedPosts = await serviceProvider.GetRequiredService<IPostModificationService>().PostProcessArchivePostsAsync(posts, cancellationToken);
        
        await serviceProvider.GetRequiredService<IPostsSynchronizationService>().SynchronizePostsAsync(webFilePaths, modifiedPosts, cancellationToken);

        await serviceProvider.GetRequiredService<IWebDataFilesRepository>().SetProcessingStatusAsync(webDataFileEntity, ProcessingStatus.Processed, cancellationToken);
        
        logger.LogInformation("Completed StackOverflow processing sub workflow for {WebFileUri}", webFilePaths.WebFileUri);
    }
}