namespace AllUsefulInformationSearch.StackOverflow.Models.XmlModels;

public class PostModel
{
    public int Id;
    public PostType PostTypeId;
    public required string Title { get; set; }
    public required string Body;
    public DateTimeOffset LastActivityDate { get; set; }
    public int? AcceptedAnswerId;
   
    public PostModel? AcceptedAnswer { get; set; }
    public int WebDataFileId { get; set; }
}