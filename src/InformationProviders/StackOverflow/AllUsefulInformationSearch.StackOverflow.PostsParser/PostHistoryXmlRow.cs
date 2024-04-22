namespace AllUsefulInformationSearch.StackOverflow.PostsParser;

public class PostHistoryXmlRow
{
    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlAttribute("PostHistoryTypeId")]
    public int PostHistoryTypeId { get; set; }

    [XmlAttribute("PostId")]
    public int PostId { get; set; }

    [XmlAttribute("RevisionGUID")]
    public string RevisionGuid { get; set; }

    [XmlElement("CreationDate")]
    public DateTimeOffset CreationDate { get; set; }

    [XmlAttribute("UserId")]
    public int UserId { get; set; }

    [XmlAttribute("Text")]
    public string Text { get; set; }

    // [XmlAttribute("ContentLicense")]
    // public string ContentLicense { get; set; } // maybe not needed
}