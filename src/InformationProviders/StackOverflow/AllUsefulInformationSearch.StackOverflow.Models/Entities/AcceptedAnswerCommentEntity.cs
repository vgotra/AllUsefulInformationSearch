namespace AllUsefulInformationSearch.StackOverflow.Models.Entities;

public class AcceptedAnswerCommentEntity : Entity<int>
{
    public string Text { get; set; } = null!;
    public DateTimeOffset ExternalCreationDate { get; set; }
    public int AcceptedAnswerId { get; set; }
    
    public AcceptedAnswerEntity AcceptedAnswer { get; set; } = null!;
    
    public int WebDataFileId { get; set; } // part of composite key
}