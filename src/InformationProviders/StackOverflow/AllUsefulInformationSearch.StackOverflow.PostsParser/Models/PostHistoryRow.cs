namespace AllUsefulInformationSearch.StackOverflow.PostsParser.Models;

[Serializable]
[XmlType(AnonymousType = true)]
public class PostHistoryRow
{
    [XmlAttribute] public int Id { get; set; }

    [XmlAttribute] public int PostHistoryTypeId { get; set; }

    [XmlAttribute] public int PostId { get; set; }

    [XmlAttribute("RevisionGUID")] public Guid RevisionGuid { get; set; }

    [XmlAttribute] public DateTimeOffset CreationDate { get; set; }

    [XmlAttribute] public int UserId { get; set; }

    [XmlAttribute] public string? Text { get; set; }

    // [XmlAttribute] public string ContentLicense { get; set; }

    [XmlAttribute] public string? Comment { get; set; }

    [XmlAttribute] public string? UserDisplayName { get; set; }
}