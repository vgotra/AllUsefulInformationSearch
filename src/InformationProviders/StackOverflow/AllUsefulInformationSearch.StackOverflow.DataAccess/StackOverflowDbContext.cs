namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public partial class StackOverflowDbContext(DbContextOptions<StackOverflowDbContext> options) : DbContext(options)
{
    public DbSet<SettingEntity> Settings { get; set; }
    public DbSet<WebDataFileEntity> WebDataFiles { get; set; }
}