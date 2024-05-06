namespace AllUsefulInformationSearch.StackOverflow.Models.XmlModels;

/// <summary>
/// Docs can be found by link: https://meta.stackexchange.com/questions/2677/database-schema-documentation-for-the-public-data-dump-and-sede/2678#2678
/// </summary>
public class CommentModel
{
    public int Id;
    public int PostId;
    public required string Text;
    public DateTimeOffset CreationDate;
}