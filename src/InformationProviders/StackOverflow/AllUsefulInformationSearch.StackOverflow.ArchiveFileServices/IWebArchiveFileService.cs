namespace AllUsefulInformationSearch.StackOverflow.PostsParser;

public interface IWebArchiveFileService
{
    Task<List<Post>> GetPostsWithCommentsAsync(IList<WebFilePaths> webFilePaths, CancellationToken cancellationToken = default);
    
    Task<List<Post>> GetPostsWithCommentsAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default);
}