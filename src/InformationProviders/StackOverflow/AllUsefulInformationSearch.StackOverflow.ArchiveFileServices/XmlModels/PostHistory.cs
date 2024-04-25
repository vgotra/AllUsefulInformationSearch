namespace AllUsefulInformationSearch.StackOverflow.PostsParser.XmlModels;

[Serializable]
[XmlRoot(Namespace = "", IsNullable = false)]
public class PostHistory
{
    [XmlElement("row", Form = XmlSchemaForm.Unqualified)]
    public List<PostHistoryRow> Items { get; set; } = new();
}