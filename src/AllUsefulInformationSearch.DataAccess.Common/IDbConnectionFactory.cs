namespace AllUsefulInformationSearch.DataAccess.Common;

public interface IDbConnectionFactory
{
    DbConnection GetDefaultDbConnection();

    Task<DbConnection> GetAndOpenDefaultDbConnection(CancellationToken cancellationToken = default);
    
    DbConnection GetDbConnection(string connectionString);
}