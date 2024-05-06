namespace AllUsefulInformationSearch.StackOverflow.Models.XmlModels;

public class PostModel
{
    public int Id;
    public PostType PostTypeId;
    public DateTimeOffset CreationDate;
    public required string Title { get; set; }
    public required string Body;
    public DateTimeOffset LastActivityDate { get; set; }
    public int? AcceptedAnswerId;
    
    public List<CommentModel> Comments { get; set; } = new();
    public PostModel? AcceptedAnswer { get; set; }
    public int WebDataFileId { get; set; }
}