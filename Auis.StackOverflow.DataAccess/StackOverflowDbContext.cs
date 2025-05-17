namespace Auis.StackOverflow.DataAccess;

public partial class StackOverflowDbContext(DbContextOptions<StackOverflowDbContext> options) : DbContext(options)
{
    public DbSet<WebDataFileEntity> WebDataFiles { get; set; } = null!;
    public DbSet<PostEntity> Posts { get; set; } = null!;
}