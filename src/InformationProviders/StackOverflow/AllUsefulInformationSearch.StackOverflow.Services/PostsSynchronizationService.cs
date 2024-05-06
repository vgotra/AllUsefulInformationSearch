using AllUsefulInformationSearch.StackOverflow.Models.Extensions;

namespace AllUsefulInformationSearch.StackOverflow.Services;

public class PostsSynchronizationService(StackOverflowDbContext dbContext, ILogger<PostsSynchronizationService> logger) : IPostsSynchronizationService
{
    public async Task SynchronizePostsAsync(List<Post> modifiedPosts, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Started synchronizing posts to database");

        //TODO Just add at current moment
        var entities = modifiedPosts.Select(x => x.ToEntity()).ToList();
        
        DisplayStatistics(entities);
        
        await dbContext.Posts.AddRangeAsync(entities, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Completed synchronizing posts to database");
    }

    private void DisplayStatistics(List<PostEntity> entities)
    {
        var files = dbContext.WebDataFiles.AsNoTracking().ToList();
        var groups = entities.GroupBy(x => x.WebDataFileId).ToList();

        foreach (var group in groups)
            logger.LogInformation("File: {File}, Posts Count: {PostsCount}", files.First(x => x.Id == group.Key).Name, group.Count());
    }
}