namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public class SettingsRepository : ISettingsRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public SettingsRepository(IDbConnectionFactory dbConnectionFactory) => _dbConnectionFactory = dbConnectionFactory;

    public async Task<List<SettingEntity>> GetAllSettingsAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = await _dbConnectionFactory.GetAndOpenDefaultDbConnection(cancellationToken);
        var result =  await connection.QueryAsync<SettingEntity>($"SELECT * FROM StackOverflow.Settings");
        return result.ToList();
    }
}