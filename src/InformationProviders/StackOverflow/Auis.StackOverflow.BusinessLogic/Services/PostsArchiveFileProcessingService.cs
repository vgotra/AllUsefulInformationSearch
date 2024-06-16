namespace Auis.StackOverflow.BusinessLogic.Services;

public class PostsArchiveFileProcessingService(
    IPostsDeserializationService postsDeserializationService,
    IPostsSynchronizationService postsSynchronizationService,
    IDbContextFactory<StackOverflowDbContext> dbContextFactory,
    IOptions<StackOverflowOptions> options,
    ILogger<PostsArchiveFileProcessingService> logger)
    : IPostsArchiveFileProcessingService
{
    public async ValueTask ProcessArchiveFileAsync(WebDataFileEntity webDataFileEntity, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var webFileInformation = webDataFileEntity.ToWebFileInformation(options.Value);

        try
        {
            logger.LogInformation("Processing started for {WebFileName}", webDataFileEntity.Name);

            await UpdateStatusAsync(dbContext, webDataFileEntity.Id, ProcessingStatus.InProgress, cancellationToken);
            var result = await postsDeserializationService.DeserializePostsAsync(webFileInformation, cancellationToken);
            await postsSynchronizationService.SynchronizeToDatabaseAsync(webFileInformation, result, cancellationToken);
            await UpdateStatusAsync(dbContext, webDataFileEntity.Id, ProcessingStatus.Processed, cancellationToken);

            logger.LogInformation("Processing completed for {WebFileName}", webDataFileEntity.Name);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error during processing of {WebFileName}", webDataFileEntity.Name);
            await UpdateStatusAsync(dbContext, webDataFileEntity.Id, ProcessingStatus.Failed, cancellationToken);
            throw;
        }
    }

    protected virtual async ValueTask UpdateStatusAsync(StackOverflowDbContext dbContext, int fileId, ProcessingStatus status, CancellationToken cancellationToken) =>
        await dbContext.WebDataFiles.Where(x => x.Id == fileId).ExecuteUpdateAsync(s => s.SetProperty(e => e.ProcessingStatus, status), cancellationToken);
}