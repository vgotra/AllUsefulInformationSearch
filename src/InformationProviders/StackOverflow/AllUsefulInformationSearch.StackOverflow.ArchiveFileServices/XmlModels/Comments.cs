namespace AllUsefulInformationSearch.StackOverflow.PostsParser.XmlModels;

[Serializable]
[XmlRoot(Namespace = "", IsNullable = false)]
public class Comments
{
    [XmlElement("row", Form = XmlSchemaForm.Unqualified)]
    public List<Comment> Items { get; set; } = new();
}