namespace AllUsefulInformationSearch.StackOverflow.PostsParser;

public class WebArchiveFileService(IFileUtilityService fileUtilityService, ILogger<WebArchiveFileService> logger) : IWebArchiveFileService
{
    private static PostType[] UsefulPostTypes { get; } = [PostType.Question, PostType.Answer, PostType.TagWiki];

    public async Task<List<Post>> GetPostsWithCommentsAsync(IList<WebFilePaths> webFilePaths, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting processing of {Count} web files", webFilePaths.Count);
        
        //TODO This about scaling (DAPR, etc if needed), some files are huge
        var posts = new ConcurrentBag<Post>();
        await Parallel.ForEachAsync(webFilePaths, cancellationToken, async (webFilePath, token) =>
        {
            var filePosts = await GetPostsWithCommentsAsync(webFilePath, token);
            filePosts.ForEach(posts.Add);
        });
        
        logger.LogInformation("Completed processing of web files");
        return posts.ToList();
    }

    public async Task<List<Post>> GetPostsWithCommentsAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Started processing of {WebFile}", webFilePaths);
        
        await fileUtilityService.DownloadFileAsync(webFilePaths, cancellationToken);
        await fileUtilityService.ExtractArchiveFileAsync(webFilePaths, cancellationToken);

        //TODO To Deserialization service
        var posts = Path.Combine(webFilePaths.ArchiveOutputDirectory, "Posts.xml").DeserializeXmlFileToList<Posts>("posts").Items;
        var commentItems = Path.Combine(webFilePaths.ArchiveOutputDirectory, "Comments.xml").DeserializeXmlFileToList<Comments>("comments").Items;

        posts.ForEach(p =>
        {
            p.Comments.AddRange(commentItems.Where(c => c.PostId == p.Id));
            p.AcceptedAnswer = posts.FirstOrDefault(x => x.Id == p.AcceptedAnswerId && x.PostTypeId == PostType.Answer);
        });

        // get only useful answered posts
        posts = posts.Where(x => UsefulPostTypes.Contains(x.PostTypeId) && x.AcceptedAnswerId != null).ToList();

        if (posts == null || posts.Count == 0)
            throw new InvalidDataException();
        
        fileUtilityService.DeleteProcessedFiles(webFilePaths); 

        logger.LogInformation("Completed processing of {WebFile}", webFilePaths);
        return posts;
    }
}