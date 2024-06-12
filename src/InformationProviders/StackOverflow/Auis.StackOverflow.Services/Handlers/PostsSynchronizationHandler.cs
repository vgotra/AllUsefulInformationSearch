namespace Auis.StackOverflow.Services.Handlers;

public class PostsSynchronizationHandler(IServiceProvider serviceProvider) : ICommandHandler<PostsSynchronizationCommand>
{
    public async ValueTask<Unit> Handle(PostsSynchronizationCommand command, CancellationToken cancellationToken)
    {
        var dbContext = serviceProvider.GetRequiredService<StackOverflowDbContext>();

        var postsToAdd = new HashSet<PostModel>();
        var postsToUpdate = new HashSet<PostModel>();

        var existingAnswers = await dbContext.AcceptedAnswers.AsNoTracking()
            .Where(x => x.PostWebDataFileId == command.WebFileInformation.WebDataFileId).Select(x => new { x.Id, x.PostId, x.ExternalLastActivityDate })
            .ToListAsync(cancellationToken);
        var existingPosts = await dbContext.Posts.AsNoTracking()
            .Where(x => x.WebDataFileId == command.WebFileInformation.WebDataFileId).Select(x => new { x.Id, x.ExternalLastActivityDate })
            .ToListAsync(cancellationToken);

        command.ModifiedPosts.ForEach(post =>
        {
            var existingPost = existingPosts.Find(x => x.Id == post.Id);
            if (existingPost == null)
            {
                postsToAdd.Add(post);
            }
            else if (existingPost.ExternalLastActivityDate < post.LastActivityDate)
            {
                postsToUpdate.Add(post);
                var existingAnswer = existingAnswers.Find(x => x.PostId == post.Id && x.Id == post.AcceptedAnswerId);
                if (existingAnswer!.ExternalLastActivityDate < post.AcceptedAnswer!.LastActivityDate)
                    postsToUpdate.Add(post);
            }
        });

        if (postsToAdd.Count > 0)
            await dbContext.Posts.AddRangeAsync(postsToAdd.Select(x => x.ToEntity()), cancellationToken);

        if (postsToUpdate.Count > 0)
        {
            var ids = postsToUpdate.Select(x => x.Id).Distinct();
            var posts = await dbContext.Posts.Include(pe => pe.AcceptedAnswer).Where(x => x.WebDataFileId == command.WebFileInformation.WebDataFileId && ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

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

        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}