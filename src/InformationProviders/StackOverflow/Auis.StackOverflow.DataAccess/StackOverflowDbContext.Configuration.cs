namespace Auis.StackOverflow.DataAccess;

public partial class StackOverflowDbContext
{
    public const string DbSchemaName = "StackOverflow";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //TODO Move those configurations to Entities for easy tracking of changes
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
            entity.Property(e => e.Question).IsRequired().HasMaxLength(-1);
            entity.Property(e => e.Answer).IsRequired().HasMaxLength(-1);
        });

        modelBuilder.Model.GetEntityTypes().ToList()
            .ForEach(e =>
            {
                e.SetSchema(DbSchemaName);
                e.GetForeignKeys().ToList().ForEach(x => x.DeleteBehavior = DeleteBehavior.NoAction);

                var lastUpdatedProperty = e.FindProperty(nameof(IUpdatableEntity.LastUpdated));
                if (lastUpdatedProperty == null || lastUpdatedProperty.ClrType != typeof(DateTimeOffset))
                    return;
                lastUpdatedProperty.ValueGenerated = ValueGenerated.OnAddOrUpdate;
                lastUpdatedProperty.SetDefaultValueSql("GETUTCDATE()");
            });
    }
}