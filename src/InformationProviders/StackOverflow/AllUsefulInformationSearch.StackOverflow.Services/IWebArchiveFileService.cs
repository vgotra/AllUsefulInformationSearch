using AllUsefulInformationSearch.StackOverflow.Models.Common;

namespace AllUsefulInformationSearch.StackOverflow.Services;

public interface IWebArchiveFileService
{
    Task<List<Post>> GetPostsWithCommentsAsync(IList<WebFilePaths> webFilePaths, CancellationToken cancellationToken = default);
    
    Task<List<Post>> GetPostsWithCommentsAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default);
}