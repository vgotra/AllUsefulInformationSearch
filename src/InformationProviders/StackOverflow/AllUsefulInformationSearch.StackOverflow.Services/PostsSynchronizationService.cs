namespace AllUsefulInformationSearch.StackOverflow.Services;

public class PostsSynchronizationService(StackOverflowDbContext dbContext, ILogger<PostsSynchronizationService> logger) : IPostsSynchronizationService
{
    public async Task SynchronizePostsAsync(WebFilePaths webFilePaths, List<Post> modifiedPosts, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Started synchronizing posts to database for {WebFileUri}", webFilePaths.WebFileUri);

        //TODO Just add at current moment
        var entities = modifiedPosts.Select(x => x.ToEntity()).ToList();
        if (entities.Count > 0)
        {
            await dbContext.Posts.AddRangeAsync(entities, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);    
        }
        
        logger.LogInformation("Completed synchronizing posts to database for {WebFileUri}", webFilePaths.WebFileUri);
    }
}