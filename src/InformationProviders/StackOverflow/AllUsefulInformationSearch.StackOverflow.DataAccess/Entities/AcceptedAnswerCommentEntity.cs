namespace AllUsefulInformationSearch.StackOverflow.DataAccess.Entities;

public class AcceptedAnswerCommentEntity : UpdatableEntity<int>
{
    public string Text { get; set; } = null!;
    public DateTimeOffset ExternalCreationDate { get; set; }
    public int AcceptedAnswerId { get; set; }
    
    public AcceptedAnswerEntity AcceptedAnswer { get; set; } = null!;
}