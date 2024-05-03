namespace AllUsefulInformationSearch.StackOverflow.Models.Entities;

public class AcceptedAnswerCommentEntity : UpdatableEntity<int>
{
    public string Text { get; set; } = null!;
    public DateTimeOffset ExternalCreationDate { get; set; }
    public int AcceptedAnswerId { get; set; }
    
    public AcceptedAnswerEntity AcceptedAnswer { get; set; } = null!;
    
    public Guid WebDataFileId { get; set; } // part of composite key
}