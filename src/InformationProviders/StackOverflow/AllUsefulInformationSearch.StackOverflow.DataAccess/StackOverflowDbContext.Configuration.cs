namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public partial class StackOverflowDbContext
{
    public const string DbSchemaName = "StackOverflow";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //TODO Move those configurations to Entities for easy tracking of changes
        modelBuilder.Entity<SettingEntity>(entity =>
        {
            entity.ToTable("Settings").HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255).HasComment("Name of the setting.");
            entity.Property(e => e.Value).IsRequired().HasMaxLength(255).HasComment("Value of the setting.");
        });

        modelBuilder.Entity<WebDataFileEntity>(entity =>
        {
            entity.ToTable("WebDataFiles").HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255).HasComment("Name of the web data file at StackOverflow archive.");
            entity.Property(e => e.Link).IsRequired().HasMaxLength(255).HasComment("Partial link to the web data file at StackOverflow archive.");
            entity.Property(e => e.ExternalLastModified).HasComment("Datetime of last modified of the web data file at StackOverflow archive.");
            entity.Property(e => e.ProcessingStatus).HasComment("Processing status of file based on enumeration in source code.");
        });

        modelBuilder.Entity<PostEntity>(entity =>
        {
            entity.ToTable("Posts").HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<PostCommentEntity>(entity =>
        {
            entity.ToTable("PostComments").HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<AcceptedAnswerEntity>(entity =>
        {
            entity.ToTable("AcceptedAnswers").HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<AcceptedAnswerCommentEntity>(entity =>
        {
            entity.ToTable("AcceptedAnswerComments").HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Model.GetEntityTypes().ToList()
            .ForEach(e =>
            {
                e.SetSchema(DbSchemaName);
                e.GetForeignKeys().ToList().ForEach(x => x.DeleteBehavior = DeleteBehavior.NoAction);
            });
    }
}