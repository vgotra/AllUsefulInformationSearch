namespace AllUsefulInformationSearch.StackOverflow.Services;

public interface IPostsSynchronizationService
{
    Task SynchronizePostsAsync(WebFilePaths webFilePaths, List<Post> modifiedPosts, CancellationToken cancellationToken = default);
}