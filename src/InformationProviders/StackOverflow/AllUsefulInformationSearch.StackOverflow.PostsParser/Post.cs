using System.Xml.Schema;

namespace AllUsefulInformationSearch.StackOverflow.PostsParser;

[Serializable]
[XmlType(AnonymousType=true)]
[XmlRoot(Namespace="", IsNullable=false)]
public class Posts {
    
    [XmlElement("row", Form=XmlSchemaForm.Unqualified)]
    public Post[]? Items { get; set; }
}

//TODO Improve this later with strong typing for each attribute
[Serializable]
[XmlType(AnonymousType=true)]
public class Post {

    [XmlAttribute]
    public string Id { get; set; }

    [XmlAttribute]
    public string PostTypeId { get; set; }

    [XmlAttribute]
    public string CreationDate { get; set; }

    [XmlAttribute]
    public string Score { get; set; }

    [XmlAttribute]
    public string ViewCount { get; set; }
    
    [XmlAttribute]
    public string Body { get; set; }
    
    [XmlAttribute]
    public string OwnerUserId { get; set; }

    [XmlAttribute]
    public string LastEditorUserId { get; set; }

    [XmlAttribute]
    public string LastEditDate { get; set; }

    [XmlAttribute]
    public string LastActivityDate { get; set; }

    [XmlAttribute]
    public string Title { get; set; }

    [XmlAttribute]
    public string Tags { get; set; }

    [XmlAttribute]
    public string AnswerCount { get; set; }

    [XmlAttribute]
    public string CommentCount { get; set; }

    //[XmlAttribute]
    //public string ContentLicense { get; set; }

    [XmlAttribute]
    public string AcceptedAnswerId { get; set; }

    [XmlAttribute]
    public string OwnerDisplayName { get; set; }

    [XmlAttribute]
    public string CommunityOwnedDate { get; set; }

    [XmlAttribute]
    public string ParentId { get; set; }

    [XmlAttribute]
    public string ClosedDate { get; set; }

    [XmlAttribute]
    public string FavoriteCount { get; set; }
}
