namespace AllUsefulInformationSearch.StackOverflow.PostsParser;

public class PostHistoryXmlFileDeserializer
{
    public static PostHistory? DeserializeXmlFileToList(string filePath)
    {
        var serializer = new XmlSerializer(typeof(PostHistory), new XmlRootAttribute("posthistory"));
        using var fileStream = new FileStream(filePath, FileMode.Open);
        var result = serializer.Deserialize(fileStream);
        return result as PostHistory;
    }
}