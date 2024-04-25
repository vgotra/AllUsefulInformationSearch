namespace AllUsefulInformationSearch.StackOverflow.PostsParser;

public class PostsXmlFileDeserializer
{
    public static Posts? DeserializeXmlFileToList(string filePath)
    {
        var serializer = new XmlSerializer(typeof(Posts), new XmlRootAttribute("posts"));
        using var fileStream = new FileStream(filePath, FileMode.Open);
        var result = serializer.Deserialize(fileStream);
        return result as Posts;
    }
}