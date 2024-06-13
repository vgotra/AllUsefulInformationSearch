using EFCore.BulkExtensions;

namespace Auis.StackOverflow.Services.Handlers;

public class PostsSynchronizationHandler(IDbContextFactory<StackOverflowDbContext> dbContextFactory, IOptions<StackOverflowOptions> options) : ICommandHandler<PostsSynchronizationCommand>
{
    public async ValueTask<Unit> Handle(PostsSynchronizationCommand command, CancellationToken cancellationToken)
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var postsToAdd = new HashSet<PostModel>();
        var postsToUpdate = new HashSet<PostModel>();

        var existingPosts = await dbContext.Posts.AsNoTracking()
            .Where(x => x.WebDataFileId == command.WebFileInformation.WebDataFileId).Select(x => new { x.Id, x.QuestionExternalLastActivityDate, x.AnswerExternalLastActivityDate })
            .ToListAsync(cancellationToken);

        command.ModifiedPosts.ForEach(post =>
        {
            var existingPost = existingPosts.Find(x => x.Id == post.Id);
            if (existingPost == null)
                postsToAdd.Add(post);
            else if (existingPost.QuestionExternalLastActivityDate < post.LastActivityDate || existingPost.AnswerExternalLastActivityDate < post.AcceptedAnswer!.LastActivityDate)
                postsToUpdate.Add(post);
        });

        if (postsToAdd.Count > 0)
        {
            if (options.Value.UseDatabaseBulkMethods)
                await dbContext.BulkInsertAsync(postsToAdd.Select(x => x.ToEntity()), cancellationToken: cancellationToken);
            else
                await dbContext.Posts.AddRangeAsync(postsToAdd.Select(x => x.ToEntity()), cancellationToken);
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
                post.Question = modifiedPost.Body;
                post.Answer = modifiedPost.AcceptedAnswer!.Body;
                post.QuestionExternalLastActivityDate = modifiedPost.LastActivityDate;
                post.AnswerExternalLastActivityDate = modifiedPost.AcceptedAnswer.LastActivityDate;
            }
        }

        if (postsToAdd.Count > 0 || postsToUpdate.Count > 0)
            await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}