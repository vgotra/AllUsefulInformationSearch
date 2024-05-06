namespace AllUsefulInformationSearch.StackOverflow.Services;

public interface IPostModificationService
{
    Task<List<PostModel>> PostProcessArchivePostsAsync(List<PostModel> posts, CancellationToken cancellationToken = default);
}