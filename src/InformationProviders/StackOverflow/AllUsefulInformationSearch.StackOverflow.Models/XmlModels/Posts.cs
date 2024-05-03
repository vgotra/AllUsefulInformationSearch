using System.Xml.Schema;
using System.Xml.Serialization;

namespace AllUsefulInformationSearch.StackOverflow.Models.XmlModels;

[Serializable]
[XmlRoot(Namespace = "", IsNullable = false)]
public class Posts
{
    [XmlElement("row", Form = XmlSchemaForm.Unqualified)]
    public List<Post> Items { get; set; } = new();
}