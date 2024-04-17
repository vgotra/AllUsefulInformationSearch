namespace AllUsefulInformationSearch.DataAccess.Common;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString) => _connectionString = connectionString; // TODO Add options later

    public NpgsqlConnection  GetDefaultDbConnection() => GetDbConnection(_connectionString);
    public async Task<NpgsqlConnection > GetAndOpenDefaultDbConnection(CancellationToken cancellationToken = default)
    {
        var dbConnection = GetDefaultDbConnection();
        await dbConnection.OpenAsync(cancellationToken);
        return dbConnection;
    }

    public NpgsqlConnection  GetDbConnection(string connectionString) => new(connectionString);
}