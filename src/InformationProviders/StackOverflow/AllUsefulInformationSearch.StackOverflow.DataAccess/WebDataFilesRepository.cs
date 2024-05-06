namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public class WebDataFilesRepository(StackOverflowDbContext dbContext) : IWebDataFilesRepository
{
    public Task<List<WebDataFileEntity>> GetWebDataFilesAsync(CancellationToken cancellationToken = default) =>
        dbContext.WebDataFiles
            .Where(x => x.IsSynchronizationEnabled && (x.ProcessingStatus == ProcessingStatus.Updated || x.ProcessingStatus == ProcessingStatus.New))
            .ToListAsync(cancellationToken);

    public async Task SetProcessingStatusAsync(WebDataFileEntity webDataFileEntity, ProcessingStatus status, CancellationToken cancellationToken = default) =>
        await dbContext.WebDataFiles.Where(x => x.Id == webDataFileEntity.Id)
            .ExecuteUpdateAsync(s => s.SetProperty(e => e.ProcessingStatus, status), cancellationToken);
}