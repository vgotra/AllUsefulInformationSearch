namespace Auis.Wikipedia.DataAccess;

public class WikipediaDbContextFactory(DbContextOptions<WikipediaDbContext> options) : IDbContextFactory<WikipediaDbContext>
{
    // Make a cached version with similar to DbContext Pooling
    public WikipediaDbContext CreateDbContext() => new(options);
}