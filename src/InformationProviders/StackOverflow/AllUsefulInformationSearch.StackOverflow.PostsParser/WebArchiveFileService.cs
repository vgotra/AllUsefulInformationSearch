namespace AllUsefulInformationSearch.StackOverflow.PostsParser;

public class WebArchiveFileService
{
    public async Task UnzipWebFileAsync(string url)
    {
        //TODO Split this to different instances: extractor of files and deserializer of files 
        using var client = new HttpClient();
        using var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        await using var archiveStream = new FileStream(Path.GetTempFileName(), FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose);
        await stream.CopyToAsync(archiveStream);
        archiveStream.Seek(0, SeekOrigin.Begin);

        var outputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        var path = Path.Combine("C:\\Program Files\\7-Zip", "7z.dll");
        SevenZipBase.SetLibraryPath(path); //TODO Find a better solution for this
        using var extractor = new SevenZipExtractor(archiveStream);
        extractor.ExtractArchive(outputDirectory);

        var posts = Path.Combine(outputDirectory, "Posts.xml").DeserializeXmlFileToList<Posts>("posts")?.Items.Where(x => x.AcceptedAnswerId != null).ToList();
        var postHistoryItems = Path.Combine(outputDirectory, "PostHistory.xml").DeserializeXmlFileToList<Posts>("posthistory")?.Items.ToList();
        var commentItems = Path.Combine(outputDirectory, "Comments.xml").DeserializeXmlFileToList<Comments>("comments")?.Items.ToList();

        if (posts == null || postHistoryItems == null || commentItems == null || posts.Count == 0 || postHistoryItems.Count == 0 || commentItems.Count == 0)
            throw new InvalidDataException();
    }
}