namespace AllUsefulInformationSearch.StackOverflow.PostsParser.XmlModels;

/// <summary>
/// Docs can be found by link: https://meta.stackexchange.com/questions/2677/database-schema-documentation-for-the-public-data-dump-and-sede/2678#2678
/// </summary>
[Serializable]
public class Comment
{
    [XmlAttribute] public int Id { get; set; }

    [XmlAttribute] public int PostId { get; set; }

    [XmlAttribute] public required string Text { get; set; }
    
    [XmlAttribute] public DateTimeOffset CreationDate { get; set; }
}