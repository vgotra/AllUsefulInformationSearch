namespace Auis.StackOverflow.Services;

public class ParsingService : IParsingService
{
    public async Task<List<PostEntity>> ParsePostsAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default)
    {
        var pathToPostsFile = Path.Combine(webFileInformation.ArchiveOutputDirectory, "Posts.xml");

        var posts = await pathToPostsFile.DeserializePostsAsync(webFileInformation.WebDataFileId, webFileInformation.WebDataFileSize, cancellationToken);

        return posts;
    }
}