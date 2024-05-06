namespace AllUsefulInformationSearch.StackOverflow.Services;

public class PostModificationService(ILogger<PostModificationService> logger) : IPostModificationService
{
    public async Task<List<PostModel>> PostProcessArchivePostsAsync(List<PostModel> posts, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting post processing of archive posts");
        await Task.CompletedTask;
        //TODO Apply cleanup if needed, filtering, etc
        logger.LogInformation("Completed post processing of archive posts");
        return posts;
    }
}