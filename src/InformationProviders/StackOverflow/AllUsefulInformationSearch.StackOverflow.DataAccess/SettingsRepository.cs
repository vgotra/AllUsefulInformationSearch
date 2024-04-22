namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public class SettingsRepository(StackOverflowDbContext dbContext) : ISettingsRepository
{
    public async Task<List<SettingEntity>> GetAllSettingsAsync(CancellationToken cancellationToken = default) => 
        await dbContext.Settings.AsNoTracking().ToListAsync(cancellationToken);
}