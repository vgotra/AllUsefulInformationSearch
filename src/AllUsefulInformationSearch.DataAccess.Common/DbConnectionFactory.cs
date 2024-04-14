namespace AllUsefulInformationSearch.DataAccess.Common;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString) => _connectionString = connectionString; // TODO Add options later

    public DbConnection GetDefaultDbConnection() => GetDbConnection(_connectionString);

    public DbConnection GetDbConnection(string connectionString) => new NpgsqlConnection(connectionString);
}