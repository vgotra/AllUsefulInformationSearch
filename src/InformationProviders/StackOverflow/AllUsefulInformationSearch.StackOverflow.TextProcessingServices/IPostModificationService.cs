namespace AllUsefulInformationSearch.StackOverflow.TextProcessingServices;

public interface IPostModificationService
{
    Task<List<Post>> PostProcessArchivePostsAsync(List<Post> posts, CancellationToken cancellationToken = default);
}