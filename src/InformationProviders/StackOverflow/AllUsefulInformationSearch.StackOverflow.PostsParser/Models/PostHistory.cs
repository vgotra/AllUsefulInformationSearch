namespace AllUsefulInformationSearch.StackOverflow.PostsParser.Models;

[Serializable]
[XmlType(AnonymousType = true)]
[XmlRoot(Namespace = "", IsNullable = false)]
public class PostHistory
{
    [XmlElement("row", Form = XmlSchemaForm.Unqualified)]
    public List<PostHistoryRow> Items { get; set; } = new();
}