namespace AllUsefulInformationSearch.DatabaseMigrations;

public interface IDatabaseMigrationService
{
    Task ApplyMigrations(CancellationToken cancellationToken = default);
}