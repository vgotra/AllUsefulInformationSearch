namespace Auis.StackOverflow.Tests.Mocks;

public class WebDataFilesRepositoryMock(StackOverflowDbContext dbContext) : IWebDataFilesRepository
{
    public Task<List<WebDataFileEntity>> GetWebDataFilesAsync(CancellationToken cancellationToken = default) =>
        dbContext.WebDataFiles
            .Where(x => x.IsSynchronizationEnabled && (x.ProcessingStatus == ProcessingStatus.Updated || x.ProcessingStatus == ProcessingStatus.New))
            .ToListAsync(cancellationToken);

    public async Task SetProcessingStatusAsync(WebDataFileEntity webDataFileEntity, ProcessingStatus status, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.WebDataFiles.FirstOrDefaultAsync(x => x.Id == webDataFileEntity.Id, cancellationToken);
        if (entity != null)
        {
            entity.ProcessingStatus = status;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}