namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public partial class StackOverflowDbContext
{
    private const string DbSchemaName = "StackOverflow";
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //TODO Move those configurations to Entities for easy tracking of changes
        modelBuilder.Entity<SettingEntity>(entity =>
        {
            entity.ToTable("Settings", DbSchemaName).HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255).HasComment("Name of the setting.");
            entity.Property(e => e.Value).IsRequired().HasMaxLength(255).HasComment("Value of the setting.");
        });

        modelBuilder.Entity<WebDataFileEntity>(entity =>
        {
            entity.ToTable("WebDataFiles", DbSchemaName).HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255).HasComment("Name of the web data file at StackOverflow archive.");
            entity.Property(e => e.Link).IsRequired().HasMaxLength(255).HasComment("Partial link to the web data file at StackOverflow archive.");
            entity.Property(e => e.ExternalLastModified).HasComment("Datetime of last modified of the web data file at StackOverflow archive.");
            entity.Property(e => e.LastUpdated).HasComment("Timestamp of when the data file entity was last updated in the database.");
            entity.Property(e => e.ProcessingStatus).HasComment("Processing status of file based on enumeration in source code.");
        });
    }
}