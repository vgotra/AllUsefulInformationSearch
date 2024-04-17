namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public interface IWebDataFilesRepository
{
    Task<List<WebDataFileEntity>> GetWebDataFileListAsync(CancellationToken cancellationToken = default);

    Task AddWebDataFileListAsync(List<WebDataFileEntity> dataFiles, CancellationToken cancellationToken = default);
    
    Task UpdateWebDataFileListAsync(List<WebDataFileEntity> dataFiles, CancellationToken cancellationToken = default);
}