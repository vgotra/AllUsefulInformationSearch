namespace AllUsefulInformationSearch.StackOverflow.Services;

public class PostsSynchronizationService(StackOverflowDbContext dbContext, ILogger<PostsSynchronizationService> logger) : IPostsSynchronizationService
{
    public async Task SynchronizePostsAsync(WebFilePaths webFilePaths, List<PostModel> modifiedPosts, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Started synchronizing posts to database for {WebFileUri}", webFilePaths.WebFileUri);

        //TODO Add syncing later 
        var entities = modifiedPosts.Select(x => x.ToEntity()).ToList();
        if (entities.Count > 0)
            await dbContext.BulkInsertAsync(entities, cancellationToken: cancellationToken);
        
        logger.LogInformation("Completed synchronizing posts to database for {WebFileUri}", webFilePaths.WebFileUri);
    }
}