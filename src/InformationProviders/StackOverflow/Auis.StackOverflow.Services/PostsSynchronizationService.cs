namespace Auis.StackOverflow.Services;

public class PostsSynchronizationService(StackOverflowDbContext dbContext, ILogger<PostsSynchronizationService> logger) : IPostsSynchronizationService
{
    public async Task SynchronizePostsAsync(WebFilePaths webFilePaths, List<PostModel> modifiedPosts, CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            logger.LogInformation("Started synchronizing posts to database for {WebFileUri}", webFilePaths.WebFileUri);

            var posts = modifiedPosts.Select(x => x.ToEntity());
            await dbContext.Posts.AddRangeAsync(posts, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Completed synchronizing posts to database for {WebFileUri}", webFilePaths.WebFileUri);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while synchronizing posts to database for {WebFileUri}", webFilePaths.WebFileUri);
            throw;
        }
        finally
        {
            sw.Stop();
            logger.LogDebug("Synchronization of {WebFileUri} took: {Elapsed}", webFilePaths.WebFileUri, sw.Elapsed);
        }
    }
}