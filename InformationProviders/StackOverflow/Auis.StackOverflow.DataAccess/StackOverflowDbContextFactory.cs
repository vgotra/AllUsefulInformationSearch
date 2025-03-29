namespace Auis.StackOverflow.DataAccess;

public class StackOverflowDbContextFactory(DbContextOptions<StackOverflowDbContext> options) : IDbContextFactory<StackOverflowDbContext>
{
    // Make a cached version with similar to DbContext Pooling
    public StackOverflowDbContext CreateDbContext() => new(options);
}