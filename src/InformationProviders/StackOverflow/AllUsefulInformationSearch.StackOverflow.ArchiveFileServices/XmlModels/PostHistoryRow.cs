namespace AllUsefulInformationSearch.StackOverflow.PostsParser.XmlModels;

/// <summary>
/// Docs can be found by link: https://meta.stackexchange.com/questions/2677/database-schema-documentation-for-the-public-data-dump-and-sede/2678#2678
/// </summary>
[Serializable]
public class PostHistoryRow
{
    [XmlAttribute] public int Id { get; set; }

    [XmlAttribute(nameof(PostHistoryTypeId))] public int PostHistoryTypeIdEnum { get; set; }
    
    [XmlIgnore] public PostHistoryType PostHistoryTypeId => (PostHistoryType)PostHistoryTypeIdEnum; // TODO Add checks for present values

    [XmlAttribute] public int PostId { get; set; }

    [XmlAttribute("RevisionGUID")] public Guid RevisionGuid { get; set; }

    [XmlAttribute] public DateTimeOffset CreationDate { get; set; }

    [XmlAttribute] public required string Text { get; set; }
}