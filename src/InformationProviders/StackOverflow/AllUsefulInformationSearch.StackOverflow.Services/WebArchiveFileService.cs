namespace AllUsefulInformationSearch.StackOverflow.Services;

public class WebArchiveFileService(IFileUtilityService fileUtilityService, ILogger<WebArchiveFileService> logger) : IWebArchiveFileService
{
    private static PostType[] UsefulPostTypes { get; } = [PostType.Question, PostType.Answer, PostType.TagWiki];

    public async Task<List<PostModel>> GetPostsWithCommentsAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Started processing of {WebFileUri}", webFilePaths.WebFileUri);

        try
        {
            await fileUtilityService.DownloadFileAsync(webFilePaths, cancellationToken);
            await fileUtilityService.ExtractArchiveFileAsync(webFilePaths, cancellationToken);

            //TODO To Deserialization service
            var posts = await Path.Combine(webFilePaths.ArchiveOutputDirectory, "Posts.xml").DeserializeXmlFileToList(webFilePaths.WebDataFileSize, XmlFileDeserializer.ParsePostXmlRow);
            posts.ForEach(p =>
            {
                p.WebDataFileId = webFilePaths.WebDataFileId;
                p.AcceptedAnswer = posts.FirstOrDefault(x => x.Id == p.AcceptedAnswerId && x.PostTypeId == PostType.Answer); // improve this 
            });

            // get only useful answered posts
            posts = posts.Where(x => UsefulPostTypes.Contains(x.PostTypeId) && x.AcceptedAnswer != null).ToList();
            if (posts == null || posts.Count == 0)
                throw new InvalidDataException();

            logger.LogInformation("Completed processing of {WebFileUri}", webFilePaths.WebFileUri);
            return posts;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error during processing of {WebFileUri}", webFilePaths.WebFileUri);
            throw;
        }
        finally
        {
            fileUtilityService.DeleteProcessedFiles(webFilePaths);
        }
    }
}