namespace Auis.StackOverflow.Common.Options;

public record StackOverflowDatabaseOptions
{
    public bool UseDatabaseBulkMethods { get; set; } = false;
    
    public string SelectedConnectionString { get; set; } = string.Empty;
}