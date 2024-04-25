namespace AllUsefulInformationSearch.StackOverflow.PostsParser.Models;

/// <summary>
/// Docs can be found by link: https://meta.stackexchange.com/questions/2677/database-schema-documentation-for-the-public-data-dump-and-sede/2678#2678
/// </summary>
[Serializable]
public class Post
{
    [XmlAttribute] public int Id { get; set; }

    [XmlAttribute(nameof(PostTypeId))] public int PostTypeIdEnum { get; set; }

    [XmlIgnore] public PostType PostTypeId => (PostType)PostTypeIdEnum; // TODO Add checks for present values

    [XmlAttribute] public DateTimeOffset CreationDate { get; set; }

    /// <summary>
    /// Rendered HTML
    /// </summary>
    [XmlAttribute]
    public required string Body { get; set; }

    [XmlAttribute(nameof(LastEditDate))] public DateTimeOffset LastEditDateNullable { get; set; }
    
    [XmlIgnore] public DateTimeOffset? LastEditDate => LastEditDateNullable == DateTimeOffset.MinValue ? null : LastEditDateNullable;

    [XmlAttribute] public DateTimeOffset LastActivityDate { get; set; }

    [XmlAttribute] public required string Title { get; set; }

    [XmlAttribute] public string? Tags { get; set; }

    [XmlAttribute(nameof(AcceptedAnswerId))] public int AcceptedAnswerIdNullable { get; set; }

    /// <summary>
    /// Only present if PostTypeId = 1
    /// </summary>
    [XmlIgnore]
    public int? AcceptedAnswerId => AcceptedAnswerIdNullable == 0 ? null : AcceptedAnswerIdNullable;
}