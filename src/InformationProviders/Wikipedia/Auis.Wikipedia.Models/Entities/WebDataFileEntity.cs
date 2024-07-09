namespace Auis.Wikipedia.Models.Entities;

public class WebDataFileEntity : Entity<int>
{
    public string Name { get; set; } = null!;
    public string Link { get; set; } = null!;
    public long Size { get; set; }
    public DateTimeOffset ExternalLastModified { get; set; }
    public ProcessingStatus ProcessingStatus { get; set; }
    public bool IsSynchronizationEnabled { get; set; } = true;
    
    public ICollection<PostEntity>? Posts { get; set; }
}