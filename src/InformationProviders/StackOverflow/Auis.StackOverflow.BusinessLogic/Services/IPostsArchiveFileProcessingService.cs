namespace Auis.StackOverflow.BusinessLogic.Services;

public interface IPostsArchiveFileProcessingService
{
    ValueTask ProcessArchiveFileAsync(WebDataFileEntity webDataFileEntity, CancellationToken cancellationToken = default);
}