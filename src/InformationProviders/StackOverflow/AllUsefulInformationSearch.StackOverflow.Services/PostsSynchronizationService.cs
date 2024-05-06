namespace AllUsefulInformationSearch.StackOverflow.Services;

public class PostsSynchronizationService(StackOverflowDbContext dbContext, ILogger<PostsSynchronizationService> logger) : IPostsSynchronizationService
{
    public async Task SynchronizePostsAsync(WebFilePaths webFilePaths, List<PostModel> modifiedPosts, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Started synchronizing posts to database for {WebFileUri}", webFilePaths.WebFileUri);

        //TODO Add syncing later 
        var posts = modifiedPosts.Select(x => x.ToEntity()).ToList();
        await dbContext.BulkInsertAsync(posts, cancellationToken: cancellationToken);
        var answers = posts.Select(x => x.AcceptedAnswer).ToList();
        await dbContext.BulkInsertAsync(answers, cancellationToken: cancellationToken);
        
        logger.LogInformation("Completed synchronizing posts to database for {WebFileUri}", webFilePaths.WebFileUri);
    }
}