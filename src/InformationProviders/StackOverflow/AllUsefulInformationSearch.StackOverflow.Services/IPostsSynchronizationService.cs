namespace AllUsefulInformationSearch.StackOverflow.Services;

public interface IPostsSynchronizationService
{
    Task SynchronizePostsAsync(List<Post> modifiedPosts, CancellationToken cancellationToken = default);
}