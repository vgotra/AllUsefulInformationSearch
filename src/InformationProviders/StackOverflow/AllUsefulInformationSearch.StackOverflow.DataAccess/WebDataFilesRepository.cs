namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public class WebDataFilesRepository(StackOverflowDbContext dbContext) : IWebDataFilesRepository
{
    public Task<List<WebDataFileEntity>> GetWebDataFilesAsync(CancellationToken cancellationToken = default) =>
        dbContext.WebDataFiles
            .Where(x => x.ProcessingStatus == ProcessingStatus.Updated || x.ProcessingStatus == ProcessingStatus.New)
            .ToListAsync(cancellationToken);

    public Task SetProcessingStatusAsync(List<WebDataFileEntity> webDataFiles, ProcessingStatus status, CancellationToken cancellationToken = default)
    {
        webDataFiles.ForEach(x => x.ProcessingStatus = status);
        dbContext.UpdateRange(webDataFiles);
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}