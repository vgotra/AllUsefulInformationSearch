namespace AllUsefulInformationSearch.DatabaseMigrations;

public class DatabaseMigrationOptions
{
    public string ConnectionString { get; set; } = null!;
    public string Provider { get; set; } = null!;
    public string MigrationFilesDirectoryPath { get; set; } = null!;
}