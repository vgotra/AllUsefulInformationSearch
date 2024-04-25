namespace AllUsefulInformationSearch.StackOverflow.PostsParser;

public class CommentsXmlFileDeserializer
{
    public static Comments? DeserializeXmlFileToList(string filePath)
    {
        var serializer = new XmlSerializer(typeof(Comments), new XmlRootAttribute("comments"));
        using var fileStream = new FileStream(filePath, FileMode.Open);
        var result = serializer.Deserialize(fileStream);
        return result as Comments;
    }
}