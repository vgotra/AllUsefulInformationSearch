namespace AllUsefulInformationSearch.StackOverflow.DataAccess.Entities;

public class PostCommentEntity : UpdatableEntity<int>
{
    public string Text { get; set; } = null!;
    public DateTimeOffset ExternalCreationDate { get; set; }
    public int PostId { get; set; }
    
    public PostEntity Post { get; set; } = null!;
    
    public Guid WebDataFileId { get; set; } // part of composite key
}