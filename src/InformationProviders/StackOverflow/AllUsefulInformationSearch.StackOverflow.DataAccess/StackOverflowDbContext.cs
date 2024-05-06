namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public partial class StackOverflowDbContext(DbContextOptions<StackOverflowDbContext> options) : DbContext(options)
{
    public DbSet<WebDataFileEntity> WebDataFiles { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<PostCommentEntity> PostComments { get; set; }
    public DbSet<AcceptedAnswerEntity> AcceptedAnswers { get; set; }
    public DbSet<AcceptedAnswerCommentEntity> AcceptedAnswerComments { get; set; }
    
}