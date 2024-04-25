namespace AllUsefulInformationSearch.StackOverflow.PostsParser.Models;

[Serializable]
[XmlType(AnonymousType = true)]
public class PostHistoryRow
{
    [XmlAttribute] public string Id { get; set; }

    [XmlAttribute] public string PostHistoryTypeId { get; set; }

    [XmlAttribute] public string PostId { get; set; }

    [XmlAttribute] public string RevisionGUID { get; set; }

    [XmlAttribute] public string CreationDate { get; set; }

    [XmlAttribute] public string UserId { get; set; }

    [XmlAttribute] public string Text { get; set; }

    // [XmlAttribute]
    // public string ContentLicense { get; set; }

    [XmlAttribute] public string Comment { get; set; }

    [XmlAttribute] public string UserDisplayName { get; set; }
}