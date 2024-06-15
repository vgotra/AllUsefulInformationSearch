using EFCore.BulkExtensions;

namespace Auis.StackOverflow.BusinessLogic.Handlers;

public class PostsSynchronizationHandler(IDbContextFactory<StackOverflowDbContext> dbContextFactory, IOptions<StackOverflowOptions> options) : ICommandHandler<PostsSynchronizationCommand>
{
    public async ValueTask<Unit> Handle(PostsSynchronizationCommand command, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var postsToAdd = new HashSet<PostEntity>(command.ModifiedPosts.Count);
        var postsToUpdate = new HashSet<PostEntity>(command.ModifiedPosts.Count);

        var existingPosts = await dbContext.Posts.AsNoTracking()
            .Where(x => x.WebDataFileId == command.WebFileInformation.WebDataFileId).Select(x => new { x.Id, x.QuestionExternalLastActivityDate, x.AnswerExternalLastActivityDate })
            .ToListAsync(cancellationToken);

        command.ModifiedPosts.ForEach(post =>
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
            var posts = await dbContext.Posts.Where(x => x.WebDataFileId == command.WebFileInformation.WebDataFileId && ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

            foreach (var post in posts)
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

        return Unit.Value;
    }
}