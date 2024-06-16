namespace Auis.StackOverflow.BusinessLogic.Services;

public interface IPostsDeserializationService
{
    ValueTask<List<PostEntity>> DeserializePostsAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default);
}