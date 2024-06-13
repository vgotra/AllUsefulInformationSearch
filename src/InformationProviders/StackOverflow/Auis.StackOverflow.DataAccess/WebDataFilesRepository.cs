namespace Auis.StackOverflow.DataAccess;

public class WebDataFilesRepository(IDbContextFactory<StackOverflowDbContext> dbContextFactory) : IWebDataFilesRepository
{
    public async Task<List<WebDataFileEntity>> GetWebDataFilesAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.WebDataFiles
            .Where(x => x.IsSynchronizationEnabled && (x.ProcessingStatus == ProcessingStatus.Updated || x.ProcessingStatus == ProcessingStatus.New))
            .ToListAsync(cancellationToken);
    }

    public async Task SetProcessingStatusAsync(WebDataFileEntity webDataFileEntity, ProcessingStatus status, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        await dbContext.WebDataFiles.Where(x => x.Id == webDataFileEntity.Id)
            .ExecuteUpdateAsync(s => s.SetProperty(e => e.ProcessingStatus, status), cancellationToken);
    }
}