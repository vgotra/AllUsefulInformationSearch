namespace AllUsefulInformationSearch.StackOverflow.TextProcessingServices;

public class PostModificationService(ILogger<PostModificationService> logger) : IPostModificationService
{
    public async Task<List<Post>> PostProcessArchivePostsAsync(List<Post> posts, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting post processing of archive posts");
        await Task.CompletedTask;
        //TODO Apply cleanup if needed, filtering, etc
        logger.LogInformation("Completed post processing of archive posts");
        return posts;
    }
}