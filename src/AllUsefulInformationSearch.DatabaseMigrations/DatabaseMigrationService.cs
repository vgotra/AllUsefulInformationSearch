namespace AllUsefulInformationSearch.DatabaseMigrations;

public class DatabaseMigrationService : IDatabaseMigrationService
{
    private readonly DatabaseMigrationOptions _options;
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<DatabaseMigrationService> _logger;

    public DatabaseMigrationService(DatabaseMigrationOptions options, IDbConnectionFactory dbConnectionFactory, ILogger<DatabaseMigrationService> logger)
    {
        _options = options;
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }
    
    public async Task ApplyMigrations(CancellationToken cancellationToken = default)
    {
        //TODO Structure, improve, add rollbacks, etc 
        _logger.LogInformation("Started applying migrations...");

        var migrations = await GetMigrationsAsync(cancellationToken);

        DbConnection? dbConnection = null;
        DbTransaction? transaction = null;
        
        try
        {
            dbConnection = _dbConnectionFactory.GetDefaultDbConnection();
            await dbConnection.OpenAsync(cancellationToken);
            
            transaction = await dbConnection.BeginTransactionAsync(IsolationLevel.Snapshot, cancellationToken);
        
            foreach (var migrationFile in migrations)
                await ExecuteMigrationAsync(dbConnection, migrationFile, cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            if (transaction != null)
                await transaction.RollbackAsync(cancellationToken);
            
            _logger.LogError(e, "Error applying migrations");
        }
        finally
        {
            transaction?.Dispose();
            dbConnection?.Dispose();
            
            _logger.LogInformation("Completed applying migrations...");
        }
    }
    
    private async Task<List<DatabaseMigrationFile>> GetMigrationsAsync(CancellationToken cancellationToken = default)
    {
        var migrations = new List<DatabaseMigrationFile>();
        var migrationFiles = Directory.GetFiles(_options.MigrationFilesDirectoryPath, "*.sql");
        await Parallel.ForEachAsync(migrationFiles, cancellationToken, async (migrationFile, token) =>
        {
            var migrationFileContent = await File.ReadAllTextAsync(migrationFile, token);
            migrations.Add(new DatabaseMigrationFile { FileName = Path.GetFileName(migrationFile), Content = migrationFileContent });
        });
        migrations = migrations.OrderBy(x => x.FileName).ToList();
        return migrations;
    }
    
    private async Task ExecuteMigrationAsync(DbConnection dbConnection, DatabaseMigrationFile migration, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Applying migration: {migration.FileName}");

        await using var dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = migration.Content;
        await dbCommand.ExecuteNonQueryAsync(cancellationToken);

        _logger.LogInformation($"Migration applied: {migration.FileName}");
    }
}