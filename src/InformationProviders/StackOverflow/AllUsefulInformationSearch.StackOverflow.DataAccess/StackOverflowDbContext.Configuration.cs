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
            entity.ToTable("Posts").HasKey(e => new { e.WebDataFileId, e.Id });
            entity.Property(e => e.Title).IsRequired().HasMaxLength(250);
            entity.Property(e => e.Text).IsRequired().HasMaxLength(-1);
            entity.Property(e => e.Tags).IsRequired().HasMaxLength(4000);
        });

        modelBuilder.Entity<PostCommentEntity>(entity =>
        {
            entity.ToTable("PostComments").HasKey(e => new { e.WebDataFileId, e.Id });
        });

        modelBuilder.Entity<AcceptedAnswerEntity>(entity =>
        {
            entity.ToTable("AcceptedAnswers").HasKey(e => new { e.PostWebDataFileId, e.Id });
            entity.Property(e => e.Text).IsRequired().HasMaxLength(-1);
        });

        modelBuilder.Entity<AcceptedAnswerCommentEntity>(entity =>
        {
            entity.ToTable("AcceptedAnswerComments").HasKey(e => new { e.WebDataFileId, e.Id });
        });

        modelBuilder.Model.GetEntityTypes().ToList()
            .ForEach(e =>
            {
                e.SetSchema(DbSchemaName);
                e.GetForeignKeys().ToList().ForEach(x => x.DeleteBehavior = DeleteBehavior.NoAction);

                var lastUpdatedProperty = e.FindProperty(nameof(IUpdatableEntity.LastUpdated));
                if (lastUpdatedProperty != null && lastUpdatedProperty.ClrType == typeof(DateTimeOffset))
                {
                    lastUpdatedProperty.ValueGenerated = ValueGenerated.OnAddOrUpdate;
                    lastUpdatedProperty.SetDefaultValueSql("GETUTCDATE()");
                }
            });
    }
}