using Auis.StackOverflow.Common.Options;

namespace Auis.StackOverflow.Tests;

public class PostsArchiveFileProcessingServiceMock(
    IPostsDeserializationService postsDeserializationService,
    IPostsSynchronizationService postsSynchronizationService,
    IDbContextFactory<StackOverflowDbContext> dbContextFactory,
    IOptions<StackOverflowOptions> options,
    ILogger<PostsArchiveFileProcessingService> logger) : PostsArchiveFileProcessingService(postsDeserializationService, postsSynchronizationService, dbContextFactory, options, logger)
{
    protected override async ValueTask UpdateStatusAsync(StackOverflowDbContext dbContext, int fileId, ProcessingStatus status, CancellationToken cancellationToken)
    {
        var entity = await dbContext.WebDataFiles.FirstOrDefaultAsync(x => x.Id == fileId, cancellationToken);
        if (entity != null)
        {
            entity.ProcessingStatus = status;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}