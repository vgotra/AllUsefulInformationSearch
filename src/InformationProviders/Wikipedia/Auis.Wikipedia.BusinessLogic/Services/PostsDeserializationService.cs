namespace Auis.Wikipedia.BusinessLogic.Services;

public class PostsDeserializationService(IFileUtilityService fileUtilityService, IPostsArchiveFileParserService overflowFileParserService) : IPostsDeserializationService
{
    public async ValueTask<List<PostEntity>> DeserializePostsAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default)
    {
        try
        {
            await fileUtilityService.ExtractArchiveFileAsync(webFileInformation, cancellationToken);
            fileUtilityService.DeleteTemporaryFiles(webFileInformation);

            var posts = await overflowFileParserService.DeserializePostsAsync(webFileInformation, cancellationToken);
            return posts;
        }
        finally
        {
            fileUtilityService.DeleteExtractedFiles(webFileInformation);
        }
    }
}