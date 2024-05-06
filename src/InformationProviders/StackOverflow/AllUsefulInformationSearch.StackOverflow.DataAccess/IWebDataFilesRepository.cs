namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public interface IWebDataFilesRepository
{
    Task<List<WebDataFileEntity>> GetWebDataFilesAsync(CancellationToken cancellationToken = default);
    
    Task SetProcessingStatusAsync(WebDataFileEntity webDataFileEntity, ProcessingStatus status, CancellationToken cancellationToken = default);
}