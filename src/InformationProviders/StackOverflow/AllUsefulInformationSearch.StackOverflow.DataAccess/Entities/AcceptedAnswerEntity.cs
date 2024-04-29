namespace AllUsefulInformationSearch.StackOverflow.DataAccess.Entities;

public class AcceptedAnswerEntity : UpdatableEntity<int> //TODO Think what can be better for primary key? ExternalId (because its unique in StackOverflow)?
{
    public string? Title { get; set; }
    public string Text { get; set; } = null!;
    public string? Tags { get; set; }
    public DateTimeOffset ExternalCreationDate { get; set; }
    public DateTimeOffset ExternalLastActivityDate { get; set; }
    
    public int PostId { get; set; }
    public Guid WebDataFileId { get; set; }
    
    public PostEntity Post { get; set; } = null!;
    public WebDataFileEntity WebDataFile { get; set; } = null!;
    
    public ICollection<AcceptedAnswerCommentEntity>? Comments { get; set; }
}