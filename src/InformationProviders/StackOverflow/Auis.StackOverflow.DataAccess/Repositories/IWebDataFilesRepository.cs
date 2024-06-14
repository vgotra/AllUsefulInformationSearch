namespace Auis.StackOverflow.DataAccess.Repositories;

public interface IWebDataFilesRepository
{
    Task<List<WebDataFileEntity>> GetWebDataFilesAsync(CancellationToken cancellationToken = default);
    
    Task SetProcessingStatusAsync(WebDataFileEntity webDataFileEntity, ProcessingStatus status, CancellationToken cancellationToken = default);
}