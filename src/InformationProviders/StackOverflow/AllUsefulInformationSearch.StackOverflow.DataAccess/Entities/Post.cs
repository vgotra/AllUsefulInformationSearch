namespace AllUsefulInformationSearch.StackOverflow.DataAccess.Entities;

public class Post : UpdatableEntity<int> //TODO Think what can be better for primary key? ExternalId (because its unique in StackOverflow)?
{
    public int ExternalId { get; set; } 
    public int PostTypeId { get; set; } //TODO Think how to use sharding/partitioning
    public string Title { get; set; } = null!;
    public string Text { get; set; } = null!;
    public string? Tags { get; set; }
    public DateTimeOffset ExternalCreationDate { get; set; }
    public DateTimeOffset ExternalLastActivityDate { get; set; }
    public int AcceptedAnswerId { get; set; }
    public Guid WebDataFileEntityId { get; set; }
}