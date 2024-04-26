namespace AllUsefulInformationSearch.StackOverflow.PostsParser;

public class WebArchiveFileService(IFileUtilityService fileUtilityService)
{
    private static PostType[] UsefulPostTypes { get; } = [PostType.Question, PostType.Answer, PostType.TagWiki];

    public async Task<List<Post>> GetPostsWithCommentsAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default)
    {
        await fileUtilityService.DownloadFileAsync(webFilePaths, cancellationToken);
        await fileUtilityService.ExtractArchiveFileAsync(webFilePaths, cancellationToken);

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
        
        fileUtilityService.DeleteProcessedFiles(webFilePaths); // TODO Maybe better to use this method in different component 

        return posts;
    }
}