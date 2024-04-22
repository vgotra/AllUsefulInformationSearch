namespace AllUsefulInformationSearch.StackOverflow.PostsParser;

public class PostHistoryXmlFileDeserializer
{
    public static List<PostHistoryXmlRow>? DeserializeXmlFileToList(string filePath)
    {
        var serializer = new XmlSerializer(typeof(List<PostHistoryXmlRow>), new XmlRootAttribute("posthistory"));
        using var fileStream = new FileStream(filePath, FileMode.Open);
        var result = serializer.Deserialize(fileStream);
        return result as List<PostHistoryXmlRow>;
    }
}