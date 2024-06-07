namespace Auis.StackOverflow.DataAccess;

public partial class StackOverflowDbContext(DbContextOptions<StackOverflowDbContext> options) : DbContext(options)
{
    public DbSet<WebDataFileEntity> WebDataFiles { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<AcceptedAnswerEntity> AcceptedAnswers { get; set; }
}