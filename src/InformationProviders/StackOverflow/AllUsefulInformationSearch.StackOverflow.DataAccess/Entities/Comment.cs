namespace AllUsefulInformationSearch.StackOverflow.DataAccess.Entities;

public class Comment : UpdatableEntity<int>
{
    public int ExternalId { get; set; }
    public string Text { get; set; } = null!;
    public DateTimeOffset ExternalCreationDate { get; set; }
    public int PostId { get; set; }
}