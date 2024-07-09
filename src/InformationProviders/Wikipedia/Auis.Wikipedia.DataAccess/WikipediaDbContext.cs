namespace Auis.Wikipedia.DataAccess;

public partial class WikipediaDbContext(DbContextOptions<WikipediaDbContext> options) : DbContext(options)
{
    public DbSet<WebDataFileEntity> WebDataFiles { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
}