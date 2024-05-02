namespace AllUsefulInformationSearch.StackOverflow.Workflows;

public interface IPostsSynchronizationService
{
    Task SynchronizePostsAsync(List<Post> modifiedPosts, CancellationToken cancellationToken = default);
}