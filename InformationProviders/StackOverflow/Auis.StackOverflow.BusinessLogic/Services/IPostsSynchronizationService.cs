namespace Auis.StackOverflow.BusinessLogic.Services;

public interface IPostsSynchronizationService
{
    ValueTask SynchronizeToDatabaseAsync(WebFileInformation webFileInformation, List<PostEntity> posts, CancellationToken cancellationToken = default);
}