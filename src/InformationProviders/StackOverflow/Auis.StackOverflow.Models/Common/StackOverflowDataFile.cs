namespace Auis.StackOverflow.Models.Common;

public record StackOverflowDataFile
{
    public string Name { get; set; } = null!;
    public string Link { get; set; } = null!;
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
}