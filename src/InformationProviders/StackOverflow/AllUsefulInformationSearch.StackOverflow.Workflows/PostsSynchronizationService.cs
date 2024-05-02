namespace AllUsefulInformationSearch.StackOverflow.Workflows;

public class PostsSynchronizationService(StackOverflowDbContext dbContext, ILogger<PostsSynchronizationService> logger) : IPostsSynchronizationService
{
    public async Task SynchronizePostsAsync(List<Post> modifiedPosts, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Started synchronizing posts to database");

        await Task.CompletedTask;
        
        logger.LogInformation("Completed synchronizing posts to database");
    }
}