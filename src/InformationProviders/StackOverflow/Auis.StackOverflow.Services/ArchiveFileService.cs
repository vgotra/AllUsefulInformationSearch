namespace Auis.StackOverflow.Services;

public class ArchiveFileService(IFileUtilityService fileUtilityService, ILogger<ArchiveFileService> logger) : IArchiveFileService
{
    private static PostType[] UsefulPostTypes { get; } = [PostType.Question, PostType.Answer, PostType.TagWiki];

    public async Task<List<PostModel>> GetPostsWithCommentsAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Started processing of {WebFileUri}", webFileInformation.FileUri);

        try
        {
            await fileUtilityService.ExtractArchiveFileAsync(webFileInformation, cancellationToken);

            //TODO To Deserialization service
            var posts = await Path.Combine(webFileInformation.ArchiveOutputDirectory, "Posts.xml").DeserializeXmlFileToList(webFileInformation.WebDataFileSize, XmlFileDeserializer.ParsePostXmlRow);
            posts.ForEach(p =>
            {
                p.WebDataFileId = webFileInformation.WebDataFileId;
                p.AcceptedAnswer = posts.FirstOrDefault(x => x.Id == p.AcceptedAnswerId && x.PostTypeId == PostType.Answer); // improve this 
            });

            // get only useful answered posts
            posts = posts.Where(x => UsefulPostTypes.Contains(x.PostTypeId) && x.AcceptedAnswer != null).ToList();
            if (posts == null || posts.Count == 0)
                throw new InvalidDataException();

            logger.LogInformation("Completed processing of {WebFileUri}", webFileInformation.FileUri);
            return posts;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error during processing of {WebFileUri}", webFileInformation.FileUri);
            throw;
        }
        finally
        {
            fileUtilityService.DeleteProcessedFiles(webFileInformation);
        }
    }
}