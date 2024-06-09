namespace Auis.StackOverflow.Services;

public interface IPostsSynchronizationService
{
    Task SynchronizePostsAsync(WebFileInformation webFileInformation, List<PostModel> modifiedPosts, CancellationToken cancellationToken = default);
}