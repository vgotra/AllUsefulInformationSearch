namespace AllDataSearch.DataAccess.Entities;

[Alias("InformationSourceDataFiles")]
public class InformationSourceDataFile
{
    //TODO Specify string length, etc later

    [AutoId, PrimaryKey]
    public Guid Id { get; set; }

    [Required]
    public InformationProvider Provider { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Link { get; set; } = null!;

    [Required]
    public long Size { get; set; }

    [Required]
    public DateTimeOffset ProviderLastModified { get; set; }

    [Required]
    public DateTimeOffset Updated { get; set; }
}