using System.Xml.Schema;
using System.Xml.Serialization;

namespace AllUsefulInformationSearch.StackOverflow.Models.XmlModels;

[Serializable]
[XmlRoot(Namespace = "", IsNullable = false)]
public class Comments
{
    [XmlElement("row", Form = XmlSchemaForm.Unqualified)]
    public List<Comment> Items { get; set; } = new();
}