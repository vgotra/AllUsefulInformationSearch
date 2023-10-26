namespace AllUsefulInformationSearch.DataAccess;

public class InformationSourceDataFileRepository : IRepository<InformationSourceDataFile>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public InformationSourceDataFileRepository(IDbConnectionFactory dbConnectionFactory) => _dbConnectionFactory = dbConnectionFactory;

    public async Task<List<InformationSourceDataFile>> GetAllAsync(CancellationToken token = default)
    {
        using var db = _dbConnectionFactory.OpenDbConnection();
        return await db.SelectAsync<InformationSourceDataFile>(token);
    }

    public async Task<List<InformationSourceDataFile>> GetAllByProviderAsync(InformationProvider provider, CancellationToken token = default)
    {
        using var db = _dbConnectionFactory.OpenDbConnection();
        return await db.SelectAsync<InformationSourceDataFile>(x => x.Provider == provider, token);
    }
}