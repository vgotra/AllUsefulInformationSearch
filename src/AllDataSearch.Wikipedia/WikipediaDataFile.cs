namespace AllDataSearch.Wikipedia;

public record WikipediaDataFile
{
    public string Name { get; set; } = null!;
    public string Link { get; set; } = null!;
    public string Size { get; set; } = null!;
    public DateTime LastModified { get; set; }
}