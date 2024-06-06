namespace Auis.StackOverflow.Services;

public interface IPostsSynchronizationService
{
    Task SynchronizePostsAsync(WebFilePaths webFilePaths, List<PostModel> modifiedPosts, CancellationToken cancellationToken = default);
}