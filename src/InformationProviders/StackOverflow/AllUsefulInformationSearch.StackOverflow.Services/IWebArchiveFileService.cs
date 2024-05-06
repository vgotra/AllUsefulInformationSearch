namespace AllUsefulInformationSearch.StackOverflow.Services;

public interface IWebArchiveFileService
{
    Task<List<Post>> GetPostsWithCommentsAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default);
}