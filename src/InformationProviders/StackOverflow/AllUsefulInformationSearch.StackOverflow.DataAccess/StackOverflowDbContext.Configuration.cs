namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public partial class StackOverflowDbContext
{
    private const string DbSchemaName = "StackOverflow";
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SettingEntity>(entity =>
        {
            entity.ToTable("Settings", DbSchemaName).HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<WebDataFileEntity>(entity =>
        {
            entity.ToTable("WebDataFiles", DbSchemaName).HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LastModified).HasComment("Datetime of last modified of the web data file at StackOverflow archive.");
            entity.Property(e => e.LastUpdated).HasComment("Timestamp of when the data file entity was last updated in the database.");
            entity.Property(e => e.ProcessingStatus).HasComment("Processing status of file based on enumeration in source code.");
        });
    }
}