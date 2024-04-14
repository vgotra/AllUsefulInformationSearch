namespace AllUsefulInformationSearch.DataAccess.Common;

public interface IDbConnectionFactory
{
    DbConnection GetDefaultDbConnection();
    
    DbConnection GetDbConnection(string connectionString);
}