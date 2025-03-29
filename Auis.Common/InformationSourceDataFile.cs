namespace Auis.Common;

public class InformationSourceDataFile : IEntity<Guid>
{
    public Guid Id { get; set; }

    public InformationProvider Provider { get; set; }

    public string Name { get; set; } = null!;

    public string Link { get; set; } = null!;

    public long Size { get; set; }

    public DateTimeOffset ProviderLastModified { get; set; }

    public DateTimeOffset Updated { get; set; }
}