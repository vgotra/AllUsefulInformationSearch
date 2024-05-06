using System.Xml;

namespace AllUsefulInformationSearch.StackOverflow.Services;

public static class XmlFileDeserializer
{
    public static async Task<List<T>> DeserializeXmlFileToList<T>(this string filePath, Func<XmlReader, T?> parseXmlRow) where T : class
    {
        var list = new List<T>();

        using var reader = XmlReader.Create(filePath, new XmlReaderSettings { Async = true, IgnoreComments = true, CloseInput = true, IgnoreWhitespace = true });
        while (await reader.ReadAsync())
        {
            if (reader.NodeType != XmlNodeType.Element || reader.Name != "row") continue;

            var row = parseXmlRow(reader);
            if (row != null)
                list.Add(row);
        }

        return list;
    }

    public static readonly Func<XmlReader, PostModel?> ParsePostXmlRow = xmlReader =>
    {
        var idVal = xmlReader.GetAttribute("Id");
        if (idVal == null) return null;
        
        var id = int.Parse(idVal);
        var postTypeId = (PostType)int.Parse(xmlReader.GetAttribute("PostTypeId")!);
        var creationDate = DateTimeOffset.Parse(xmlReader.GetAttribute("CreationDate")!);
        var body = xmlReader.GetAttribute("Body") ?? string.Empty;
        var lastActivityDate = DateTimeOffset.Parse(xmlReader.GetAttribute("LastActivityDate")!);
        var title = xmlReader.GetAttribute("Title") ?? string.Empty;
        var acceptedAnswerId = xmlReader.GetAttribute("AcceptedAnswerId") != null ? int.Parse(xmlReader.GetAttribute("AcceptedAnswerId")!) : (int?)null;

        var post = new PostModel
        {
            Id = id,
            PostTypeId = postTypeId,
            CreationDate = creationDate,
            Body = body,
            LastActivityDate = lastActivityDate,
            Title = title,
            AcceptedAnswerId = acceptedAnswerId
        };

        return post;
    };
}