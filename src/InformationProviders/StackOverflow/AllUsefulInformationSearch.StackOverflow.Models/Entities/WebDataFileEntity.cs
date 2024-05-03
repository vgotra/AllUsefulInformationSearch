namespace AllUsefulInformationSearch.StackOverflow.Models.Entities;

public class WebDataFileEntity : UpdatableEntity<Guid>
{
    public string Name { get; set; } = null!;
    public string Link { get; set; } = null!;
    public long Size { get; set; }
    public DateTimeOffset ExternalLastModified { get; set; }
    public ProcessingStatus ProcessingStatus { get; set; }
    
    public ICollection<PostEntity>? Posts { get; set; }
}