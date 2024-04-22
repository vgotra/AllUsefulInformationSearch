namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public class WebDataFilesRepository(StackOverflowDbContext dbContext) : IWebDataFilesRepository
{
    public async Task<List<WebDataFileEntity>> GetWebDataFileListAsync(CancellationToken cancellationToken = default) => 
        await dbContext.WebDataFiles.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddWebDataFileListAsync(List<WebDataFileEntity> dataFiles, CancellationToken cancellationToken = default)
    {
        await dbContext.WebDataFiles.AddRangeAsync(dataFiles, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateWebDataFileListAsync(List<WebDataFileEntity> dataFiles, CancellationToken cancellationToken = default)
    {
        dbContext.WebDataFiles.UpdateRange(dataFiles);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}