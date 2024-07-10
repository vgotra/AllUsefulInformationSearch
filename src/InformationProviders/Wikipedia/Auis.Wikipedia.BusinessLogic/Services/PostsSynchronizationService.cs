using EFCore.BulkExtensions;

namespace Auis.Wikipedia.BusinessLogic.Services;

public class PostsSynchronizationService(IDbContextFactory<WikipediaDbContext> dbContextFactory, IOptions<WikipediaOptions> options) : IPostsSynchronizationService
{
    public async ValueTask SynchronizeToDatabaseAsync(WebFileInformation webFileInformation, List<PostEntity> posts, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var postsToAdd = new HashSet<PostEntity>(posts.Count);
        var postsToUpdate = new HashSet<PostEntity>(posts.Count);

        var existingPosts = await dbContext.Posts.AsNoTracking()
            .Where(x => x.WebDataFileId == webFileInformation.WebDataFileId)
            .Select(x => new { x.Id, x.QuestionExternalLastActivityDate, x.AnswerExternalLastActivityDate })
            .ToListAsync(cancellationToken);

        posts.ForEach(post =>
        {
            var existingPost = existingPosts.Find(x => x.Id == post.Id);
            if (existingPost == null)
                postsToAdd.Add(post);
            else if (existingPost.QuestionExternalLastActivityDate < post.QuestionExternalLastActivityDate || existingPost.AnswerExternalLastActivityDate < post.AnswerExternalLastActivityDate)
                postsToUpdate.Add(post);
        });

        if (postsToAdd.Count > 0)
        {
            if (options.Value.UseDatabaseBulkMethods)
                await dbContext.BulkInsertAsync(postsToAdd, cancellationToken: cancellationToken);
            else
                await dbContext.Posts.AddRangeAsync(postsToAdd, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        if (postsToUpdate.Count > 0)
        {
            var ids = postsToUpdate.Select(x => x.Id).Distinct();
            var postsInDatabase = await dbContext.Posts.Where(x => x.WebDataFileId == webFileInformation.WebDataFileId && ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

            foreach (var post in postsInDatabase)
            {
                var modifiedPost = postsToUpdate.First(x => x.Id == post.Id);
                post.Title = modifiedPost.Title;
                post.Question = modifiedPost.Question;
                post.Answer = modifiedPost.Answer;
                post.QuestionExternalLastActivityDate = modifiedPost.QuestionExternalLastActivityDate;
                post.AnswerExternalLastActivityDate = modifiedPost.AnswerExternalLastActivityDate;
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}