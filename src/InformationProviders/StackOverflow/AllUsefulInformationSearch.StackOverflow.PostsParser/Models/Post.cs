namespace AllUsefulInformationSearch.StackOverflow.PostsParser.Models;

//TODO Improve handling optional attributes
[Serializable]
[XmlType(AnonymousType = true)]
public class Post
{
    [XmlAttribute] public int Id { get; set; }

    [XmlAttribute] public int PostTypeId { get; set; }

    [XmlAttribute] public DateTimeOffset CreationDate { get; set; }

    // [XmlAttribute] public int Score { get; set; }

    // [XmlAttribute] public int ViewCount { get; set; }

    [XmlAttribute] public string? Body { get; set; }

    [XmlAttribute] public int OwnerUserId { get; set; }

    [XmlAttribute] public int LastEditorUserId { get; set; }

    [XmlAttribute] public DateTimeOffset LastEditDate { get; set; }

    [XmlAttribute] public DateTimeOffset LastActivityDate { get; set; }

    [XmlAttribute] public string? Title { get; set; }

    [XmlAttribute] public string? Tags { get; set; }

    // [XmlAttribute] public int AnswerCount { get; set; }

    // [XmlAttribute] public int CommentCount { get; set; }

    //[XmlAttribute] public string ContentLicense { get; set; }

    [XmlAttribute] public int AcceptedAnswerId { get; set; }

    [XmlAttribute] public string? OwnerDisplayName { get; set; }

    [XmlAttribute] public DateTimeOffset CommunityOwnedDate { get; set; }

    [XmlAttribute] public int ParentId { get; set; }

    [XmlAttribute] public DateTimeOffset ClosedDate { get; set; }

    // [XmlAttribute] public int FavoriteCount { get; set; }
}