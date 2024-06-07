namespace Auis.StackOverflow.Services;

public class PostsSynchronizationService(StackOverflowDbContext dbContext, ILogger<PostsSynchronizationService> logger) : IPostsSynchronizationService
{
    public async Task SynchronizePostsAsync(WebFilePaths webFilePaths, List<PostModel> modifiedPosts, CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            logger.LogInformation("Started synchronizing posts to database for {WebFileUri}", webFilePaths.WebFileUri);

            var existingAnswers = await dbContext.AcceptedAnswers.AsNoTracking().Where(x => x.PostWebDataFileId == webFilePaths.WebDataFileId).Select(x => new { x.Id, x.PostId, x.ExternalLastActivityDate }).ToListAsync();
            var existingPosts = await dbContext.Posts.AsNoTracking().Where(x => x.WebDataFileId == webFilePaths.WebDataFileId).Select(x => new { x.Id, x.ExternalLastActivityDate }).ToListAsync();

            // We don't care about posts without answers            
            var postsToAdd = new List<PostModel>();
            var postsToUpdate = new List<PostModel>();
            var answersToUpdate = new List<(PostModel Post, PostModel Answer)>();

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
                        answersToUpdate.Add((post, post.AcceptedAnswer));
                }
            }

            if (postsToAdd.Count > 0)
            {
                await dbContext.Posts.AddRangeAsync(postsToAdd.Select(x => x.ToEntity()));
                logger.LogDebug("Added {Count} new posts to database for {WebFileUri}", postsToAdd.Count, webFilePaths.WebFileUri);
            }

            //TODO Improve this later
            if (answersToUpdate.Count > 0)
            {
                var answerIds = answersToUpdate.Select(x => x.Answer.Id).ToList();
                var postIds = answersToUpdate.Select(x => x.Post.Id).ToList();
                var answers = await dbContext.AcceptedAnswers.Where(x => x.PostWebDataFileId == webFilePaths.WebDataFileId && answerIds.Contains(x.Id) && postIds.Contains(x.PostId)).ToListAsync();
                foreach (var answer in answers)
                {
                    var modifiedAnswer = answersToUpdate.First(x => x.Answer.Id == answer.Id);
                    answer.Text = modifiedAnswer.Answer.Body;
                    answer.ExternalLastActivityDate = modifiedAnswer.Answer.LastActivityDate;
                }
                logger.LogDebug("Updated {Count} answers for {WebFileUri}", answersToUpdate.Count, webFilePaths.WebFileUri);
            }

            if (postsToUpdate.Count > 0)
            {
                var ids = postsToUpdate.Select(x => x.Id).ToList();
                var posts = await dbContext.Posts.Where(x => x.WebDataFileId == webFilePaths.WebDataFileId && ids.Contains(x.Id)).ToListAsync();
                foreach (var post in posts)
                {
                    var modifiedPost = postsToUpdate.First(x => x.Id == post.Id);
                    post.Text = modifiedPost.Body;
                    post.Title = modifiedPost.Title;
                    post.ExternalLastActivityDate = modifiedPost.LastActivityDate;
                }
                logger.LogDebug("Updated {Count} posts for {WebFileUri}", answersToUpdate.Count, webFilePaths.WebFileUri);
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Completed synchronizing posts to database for {WebFileUri}", webFilePaths.WebFileUri);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while synchronizing posts to database for {WebFileUri}", webFilePaths.WebFileUri);
            throw;
        }
        finally
        {
            sw.Stop();
            logger.LogDebug("Synchronization of {WebFileUri} took: {Elapsed}", webFilePaths.WebFileUri, sw.Elapsed);
        }
    }
}