﻿namespace AllUsefulInformationSearch.StackOverflow.PostsParser.XmlModels;

[Serializable]
[XmlRoot(Namespace = "", IsNullable = false)]
public class Posts
{
    [XmlElement("row", Form = XmlSchemaForm.Unqualified)]
    public List<Post> Items { get; set; } = new();
}