namespace Auis.Wikipedia.Models.Common;

public sealed record WebDataFile
{
    public string Name { get; set; } = null!;
    public string Link { get; set; } = null!;
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
}