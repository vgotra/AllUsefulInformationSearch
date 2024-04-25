namespace AllUsefulInformationSearch.StackOverflow.PostsParser;

public class WebArchiveFileService
{
    //TODO Improve it later
    public async Task UnzipWebFileAsync(string url)
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        await using var archiveStream = new FileStream(Path.GetTempFileName(), FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose);
        await stream.CopyToAsync(archiveStream);
        archiveStream.Seek(0, SeekOrigin.Begin);

        var outputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        var path = Path.Combine("C:\\Program Files\\7-Zip", "7z.dll");
        SevenZipBase.SetLibraryPath(path);
        using var extractor = new SevenZipExtractor(archiveStream);
        extractor.ExtractArchive(outputDirectory);

        var postsFile = Path.Combine(outputDirectory, "Posts.xml");
        var postHistoryFile = Path.Combine(outputDirectory, "PostHistory.xml");

        var posts = PostsXmlFileDeserializer.DeserializeXmlFileToList(postsFile)?.Items?.Where(x => x.AcceptedAnswerId != null).ToList();
        var postHistoryItems = PostHistoryXmlFileDeserializer.DeserializeXmlFileToList(postHistoryFile);

        if (posts == null || postHistoryItems == null)
            throw new InvalidDataException();
    }
}