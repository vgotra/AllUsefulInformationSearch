namespace Auis.StackOverflow.Services;

public interface IArchiveFileService
{
    Task<List<PostModel>> GetPostsWithCommentsAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default);
}