namespace Auis.StackOverflow.Models.Entities;

public class PostCommentEntity : Entity<int>
{
    public string Text { get; set; } = null!;
    public DateTimeOffset ExternalCreationDate { get; set; }
    public int PostId { get; set; }
    public int WebDataFileId { get; set; } // part of composite key
    
    public PostEntity Post { get; set; } = null!;
}