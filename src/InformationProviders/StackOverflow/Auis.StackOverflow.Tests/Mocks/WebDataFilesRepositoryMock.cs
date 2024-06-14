using Auis.StackOverflow.DataAccess.Repositories;

namespace Auis.StackOverflow.Tests.Mocks;

public class WebDataFilesRepositoryMock(IDbContextFactory<StackOverflowDbContext> dbContextFactory) : IWebDataFilesRepository
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
        var entity = await dbContext.WebDataFiles.FirstOrDefaultAsync(x => x.Id == webDataFileEntity.Id, cancellationToken);
        if (entity != null)
        {
            entity.ProcessingStatus = status;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}