namespace AllUsefulInformationSearch.DataAccess.Common;

public interface IDbConnectionFactory
{
    NpgsqlConnection GetDefaultDbConnection();

    Task<NpgsqlConnection> GetAndOpenDefaultDbConnection(CancellationToken cancellationToken = default);
    
    NpgsqlConnection GetDbConnection(string connectionString);
}