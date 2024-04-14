namespace AllUsefulInformationSearch.StackOverflow.DataAccess.Entities;

public class DataFileEntity : Entity<Guid>
{
    public string Name { get; set; } = null!;
    public string Link { get; set; } = null!;
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public ProcessingStatus ProcessingStatus { get; set; }
}