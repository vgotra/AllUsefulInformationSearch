namespace Auis.StackOverflow.DataAccess;

public class StackOverflowDbContextFactory(DbContextOptionsBuilder<StackOverflowDbContext> optionsBuilder) : IDbContextFactory<StackOverflowDbContext>
{
    // Make a cached version with similar to DbContext Pooling
    public StackOverflowDbContext CreateDbContext() => new(optionsBuilder.Options);
}