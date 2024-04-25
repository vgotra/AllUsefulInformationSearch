namespace AllUsefulInformationSearch.StackOverflow.PostsParser;

public class WebArchiveFileService(IFileUtilityService fileUtilityService)
{
    public async Task UnzipWebFileAsync(string uri, CancellationToken cancellationToken = default)
    {
        //TODO Make some paths provider and different options
        var temporaryDownloadPath = Path.GetTempFileName();
        var outputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());;
        
        await fileUtilityService.DownloadFileAsync(uri, temporaryDownloadPath, cancellationToken);
        await fileUtilityService.UnarchiveFileAsync(temporaryDownloadPath, outputDirectory, cancellationToken);
        
        var posts = Path.Combine(outputDirectory, "Posts.xml").DeserializeXmlFileToList<Posts>("posts")?.Items.Where(x => x.AcceptedAnswerId != null).ToList();
        var postHistoryItems = Path.Combine(outputDirectory, "PostHistory.xml").DeserializeXmlFileToList<Posts>("posthistory")?.Items.ToList();
        var commentItems = Path.Combine(outputDirectory, "Comments.xml").DeserializeXmlFileToList<Comments>("comments")?.Items.ToList();

        if (posts == null || postHistoryItems == null || commentItems == null || posts.Count == 0 || postHistoryItems.Count == 0 || commentItems.Count == 0)
            throw new InvalidDataException();
    }
}