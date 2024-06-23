namespace Auis.StackOverflow.Common.Options;

public class StackOverflowOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public bool IsNetworkShare { get; set; }
    public string NetworkShareBasePath { get; set; } = string.Empty;
    public bool UseSubProcessForProcessingFile { get; set; } = false;
    public string SubProcessFileName { get; set; } = string.Empty;
    public bool UseDatabaseBulkMethods { get; set; } = false;
}