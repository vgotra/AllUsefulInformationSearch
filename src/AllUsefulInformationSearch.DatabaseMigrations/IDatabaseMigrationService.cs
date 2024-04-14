namespace AllUsefulInformationSearch.DatabaseMigrations;

public interface IDatabaseMigrationService
{
    Task ApplyMigrationsAsync(CancellationToken cancellationToken = default);
}