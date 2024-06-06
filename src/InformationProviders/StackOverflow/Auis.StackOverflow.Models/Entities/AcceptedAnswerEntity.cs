namespace Auis.StackOverflow.Models.Entities;

public class AcceptedAnswerEntity : Entity<int> //TODO Think what can be better for primary key? ExternalId (because its unique in StackOverflow)?
{
    public string Text { get; set; } = null!;
    public DateTimeOffset ExternalLastActivityDate { get; set; }

    public int PostId { get; set; } //TODO Think how to optimize FK with CK
    public int PostWebDataFileId { get; set; }

    public PostEntity Post { get; set; } = null!;

    public ICollection<AcceptedAnswerCommentEntity>? Comments { get; set; }
}