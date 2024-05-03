namespace AllUsefulInformationSearch.StackOverflow.Services;

public interface IPostModificationService
{
    Task<List<Post>> PostProcessArchivePostsAsync(List<Post> posts, CancellationToken cancellationToken = default);
}