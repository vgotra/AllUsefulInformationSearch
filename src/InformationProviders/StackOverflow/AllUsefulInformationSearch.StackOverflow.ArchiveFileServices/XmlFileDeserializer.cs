namespace AllUsefulInformationSearch.StackOverflow.PostsParser;

public static class XmlFileDeserializer
{
    public static T DeserializeXmlFileToList<T>(this string filePath, string xmlRootName) where T : class
    {
        var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootName));
        using var fileStream = new FileStream(filePath, FileMode.Open);
        var obj = serializer.Deserialize(fileStream);
        if (obj is not T result)
            throw new SerializationException($"Cannot deserialize the file to the specified type {typeof(T)}.");
        return result;
    }
}