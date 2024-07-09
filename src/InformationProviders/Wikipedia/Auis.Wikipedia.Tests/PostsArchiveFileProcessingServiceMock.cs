using Auis.Wikipedia.Common.Options;

namespace Auis.Wikipedia.Tests;

public class PostsArchiveFileProcessingServiceMock(
    IPostsDeserializationService postsDeserializationService,
    IPostsSynchronizationService postsSynchronizationService,
    IDbContextFactory<WikipediaDbContext> dbContextFactory,
    IOptions<WikipediaOptions> options,
    ILogger<PostsArchiveFileProcessingService> logger) : PostsArchiveFileProcessingService(postsDeserializationService, postsSynchronizationService, dbContextFactory, options, logger)
{
    protected override async ValueTask UpdateStatusAsync(WikipediaDbContext dbContext, int fileId, ProcessingStatus status, CancellationToken cancellationToken)
    {
        var entity = await dbContext.WebDataFiles.FirstOrDefaultAsync(x => x.Id == fileId, cancellationToken);
        if (entity != null)
        {
            entity.ProcessingStatus = status;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}