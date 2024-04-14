namespace AllUsefulInformationSearch.StackOverflow.DataAccess.Entities;

public class WebDataFileEntity : Entity<Guid>
{
    public string Name { get; set; } = null!;
    public string Link { get; set; } = null!;
    public long Size { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public ProcessingStatus ProcessingStatus { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
}