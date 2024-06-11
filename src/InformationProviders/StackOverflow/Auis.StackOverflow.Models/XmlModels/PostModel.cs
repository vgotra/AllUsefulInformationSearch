namespace Auis.StackOverflow.Models.XmlModels;

public class PostModel
{
    public int Id;
    public PostType PostTypeId;
    public required string Title;
    public required string Body;
    public DateTimeOffset LastActivityDate;
    public int? AcceptedAnswerId;

    public PostModel? AcceptedAnswer;
    public int WebDataFileId;
}