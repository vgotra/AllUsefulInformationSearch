using AllUsefulInformationSearch.StackOverflow.Models.Common;

namespace AllUsefulInformationSearch.StackOverflow.Workflows;

public class StackOverflowProcessingWorkflow(IServiceProvider serviceProvider, ILogger<StackOverflowProcessingWorkflow> logger) : IWorkflow
{
    //TODO Refactor later, SOLID, tracking or errors for every web files, etc.
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Starting StackOverflow processing workflow");

            // Step 1: Sync web data files to db
            await serviceProvider.GetRequiredService<IWebDataFilesService>().SynchronizeWebDataFilesAsync(cancellationToken);

            // Step 2: Get updated/new files list from db
            var webFilesRepository = serviceProvider.GetRequiredService<IWebDataFilesRepository>();
            var webFiles = await webFilesRepository.GetWebDataFilesAsync(cancellationToken);
            var webFilePaths = webFiles
                .Select(x => new WebFilePaths { WebFileUri = x.Link, TemporaryDownloadPath = Path.GetTempFileName(), ArchiveOutputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()) })
                .ToList();

            // Step 3: Download/extract/deserialize/cleanup files (to minimize disk space usage)
            var posts = await serviceProvider.GetRequiredService<IWebArchiveFileService>().GetPostsWithCommentsAsync(webFilePaths, cancellationToken);

            // Step 4: Process deserialized data in parallel (cleanup text, etc.)
            var modifiedPosts = await serviceProvider.GetRequiredService<IPostModificationService>().PostProcessArchivePostsAsync(posts, cancellationToken);

            // Step 5: Save processed data to db
            await serviceProvider.GetRequiredService<IPostsSynchronizationService>().SynchronizePostsAsync(modifiedPosts, cancellationToken);

            // Step 6: Post processing of data, etc
            //TODO Send events to other services (AI, Full Test Search, etc.)
            await webFilesRepository.SetProcessingStatusAsync(webFiles, ProcessingStatus.Processed, cancellationToken);
            
            // Step 7: Summary
            //TODO Maybe some summary to db or similar, etc.

            logger.LogInformation("Completed StackOverflow processing workflow");
        }
        catch (Exception e)
        {
            //TODO Update web files processing status to failed status
            logger.LogCritical(e, "Error during StackOverflow processing workflow");
            throw;
        }
    }
}