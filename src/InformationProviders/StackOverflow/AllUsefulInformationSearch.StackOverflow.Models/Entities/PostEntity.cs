namespace AllUsefulInformationSearch.StackOverflow.Models.Entities;

public class PostEntity : UpdatableEntity<int> //TODO Think what can be better for primary key? ExternalId (because its unique in StackOverflow)?
{
    public string Title { get; set; } = null!;
    public string Text { get; set; } = null!;

    public string? Tags { get; set; }

    //public DateTimeOffset ExternalCreationDate { get; set; } //NOTE Not sure that I need it
    public DateTimeOffset ExternalLastActivityDate { get; set; }

    public int WebDataFileId { get; set; }

    public AcceptedAnswerEntity? AcceptedAnswer { get; set; }
    public WebDataFileEntity WebDataFile { get; set; } = null!;

    public ICollection<PostCommentEntity>? Comments { get; set; }
}