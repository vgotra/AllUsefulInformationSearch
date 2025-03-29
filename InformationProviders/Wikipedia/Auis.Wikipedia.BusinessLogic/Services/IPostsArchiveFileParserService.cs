namespace Auis.Wikipedia.BusinessLogic.Services;

public interface IPostsArchiveFileParserService
{
    Task<List<PostEntity>> DeserializePostsAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default);
}