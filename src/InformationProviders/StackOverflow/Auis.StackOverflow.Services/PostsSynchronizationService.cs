namespace Auis.StackOverflow.Services;

public class PostsSynchronizationService(StackOverflowDbContext dbContext, ILogger<PostsSynchronizationService> logger) : IPostsSynchronizationService
{
    private static readonly ActivitySource TracingSource = new("PostsSynchronizationService");

    public async Task SynchronizePostsAsync(WebFilePaths webFilePaths, List<PostModel> modifiedPosts, CancellationToken cancellationToken = default)
    {
        var postsToAdd = new List<PostModel>();
        var postsToUpdate = new List<PostModel>();

        await TracingSource.ExecuteWithTracingAsync("SynchronizePostsAsync", async (_, token) =>
        {
            logger.LogInformation("Started synchronizing posts to database for {WebFileUri}", webFilePaths.WebFileUri);

            var existingAnswers = await dbContext.AcceptedAnswers.AsNoTracking()
                .Where(x => x.PostWebDataFileId == webFilePaths.WebDataFileId).Select(x => new { x.Id, x.PostId, x.ExternalLastActivityDate })
                .ToListAsync(token);
            var existingPosts = await dbContext.Posts.AsNoTracking()
                .Where(x => x.WebDataFileId == webFilePaths.WebDataFileId).Select(x => new { x.Id, x.ExternalLastActivityDate })
                .ToListAsync(token);

            foreach (var post in modifiedPosts)
            {
                var existingPost = existingPosts.Find(x => x.Id == post.Id);
                if (existingPost == null)
                    postsToAdd.Add(post);
                else if (existingPost.ExternalLastActivityDate < post.LastActivityDate)
                {
                    postsToUpdate.Add(post);
                    var existingAnswer = existingAnswers.Find(x => x.PostId == post.Id && x.Id == post.AcceptedAnswerId);
                    if (existingAnswer.ExternalLastActivityDate < post.AcceptedAnswer.LastActivityDate)
                        postsToUpdate.Add(post);
                }
            }

            if (postsToAdd.Count > 0)
                await dbContext.Posts.AddRangeAsync(postsToAdd.Select(x => x.ToEntity()), token);

            if (postsToUpdate.Count > 0)
            {
                var ids = postsToUpdate.Select(x => x.Id).ToList();
                var posts = await dbContext.Posts.Include(pe => pe.AcceptedAnswer).Where(x => x.WebDataFileId == webFilePaths.WebDataFileId && ids.Contains(x.Id)).ToListAsync(token);

                foreach (var post in posts)
                {
                    var modifiedPost = postsToUpdate.First(x => x.Id == post.Id);
                    post.Text = modifiedPost.Body;
                    post.Title = modifiedPost.Title;
                    post.ExternalLastActivityDate = modifiedPost.LastActivityDate;
                    post.AcceptedAnswer.Text = modifiedPost.AcceptedAnswer!.Body;
                    post.AcceptedAnswer.ExternalLastActivityDate = modifiedPost.AcceptedAnswer.LastActivityDate;
                }
            }

            await dbContext.SaveChangesAsync(token);
            logger.LogInformation("Completed synchronizing posts to database for {WebFileUri}", webFilePaths.WebFileUri);
        }, activity =>
        {
            activity?.SetTag("WebFileUri", webFilePaths.WebFileUri);
            activity?.SetTag("AddedPosts", postsToAdd.Count);
            activity?.SetTag("UpdatedPosts", postsToUpdate.Count);
        }, cancellationToken);
    }
}