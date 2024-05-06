namespace AllUsefulInformationSearch.StackOverflow.Services;

public interface IWebArchiveFileService
{
    Task<List<PostModel>> GetPostsWithCommentsAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default);
}