using System.Xml;

namespace AllUsefulInformationSearch.StackOverflow.Services;

public static class XmlFileDeserializer
{
    public static async Task<List<T>> DeserializeXmlFileToList<T>(this string filePath, long fileSize, Func<XmlReader, T> parseXmlRow) where T : class
    {
        var list = new List<T>(fileSize.GetApproximateItemsCount());

        using var reader = XmlReader.Create(filePath, new XmlReaderSettings { Async = true, IgnoreComments = true, CloseInput = true, IgnoreWhitespace = true });
        while (await reader.ReadAsync())
        {
            if (reader.NodeType != XmlNodeType.Element || reader.Name != "row") continue;

            var row = parseXmlRow(reader);
            list.Add(row);
        }

        return list;
    }
    
    private static int GetApproximateItemsCount(this long fileSize) => (int)(fileSize / FileSize.Mb * 100);

    public static readonly Func<XmlReader, PostModel> ParsePostXmlRow = xmlReader =>
    {
        var id = int.Parse(xmlReader.GetAttribute(nameof(PostModel.Id))!);
        var postTypeId = (PostType)int.Parse(xmlReader.GetAttribute(nameof(PostModel.PostTypeId))!);
        var body = xmlReader.GetAttribute(nameof(PostModel.Body)) ?? string.Empty;
        var lastActivityDate = DateTimeOffset.Parse(xmlReader.GetAttribute(nameof(PostModel.LastActivityDate))!);
        var title = xmlReader.GetAttribute(nameof(PostModel.Title)) ?? string.Empty;
        
        var acceptedAnswerIdVal = xmlReader.GetAttribute(nameof(PostModel.AcceptedAnswerId));
        var acceptedAnswerId = acceptedAnswerIdVal == null ? (int?)null : int.Parse(acceptedAnswerIdVal);

        return new PostModel
        {
            Id = id,
            Title = title,
            Body = body,
            PostTypeId = postTypeId,
            LastActivityDate = lastActivityDate,
            AcceptedAnswerId = acceptedAnswerId
        };
    };
}