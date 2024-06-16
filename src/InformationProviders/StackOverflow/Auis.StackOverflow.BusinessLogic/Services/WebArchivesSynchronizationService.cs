namespace Auis.StackOverflow.BusinessLogic.Services;

public class WebArchivesSynchronizationService(IWebArchiveParserService webArchiveParserService, IDbContextFactory<StackOverflowDbContext> dbContextFactory, ILogger<WebArchivesSynchronizationService> logger)
    : IWebArchivesSynchronizationService
{
    public async ValueTask SynchronizeWebArchiveFiles(CancellationToken cancellationToken = default)
    {
        var result = await webArchiveParserService.GetWebDataFilesAsync(cancellationToken);
        await SaveToDatabaseAsync(result, cancellationToken);
    }

    private async ValueTask SaveToDatabaseAsync(List<WebDataFile> files, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var dataFiles = await dbContext.WebDataFiles.ToListAsync(cancellationToken);

        var webFiles = GetWebDataFilesToSync(files, dataFiles);
        if (webFiles.Count > 0)
        {
            await dbContext.AddRangeAsync(webFiles, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        logger.LogInformation("Web archive files synchronized");
    }

    private static List<WebDataFileEntity> GetWebDataFilesToSync(List<WebDataFile> files, List<WebDataFileEntity> dataFiles)
    {
        var archiveFileDict = files.ToDictionary(x => x.Name, x => x);
        var dataFileDict = dataFiles.ToDictionary(x => x.Name, x => x);

        var newFiles = archiveFileDict.Values.Where(x => !dataFileDict.ContainsKey(x.Name));
        var updatedFiles = dataFileDict.Values.Where(x => archiveFileDict.ContainsKey(x.Name) && x.ExternalLastModified != archiveFileDict[x.Name].LastModified).ToList();
        // No need to delete some data files, because they can be useful

        updatedFiles.ForEach(x => x.ProcessingStatus = ProcessingStatus.Updated);
        var webFiles = newFiles.Select(x => x.ToEntity()).Concat(updatedFiles).ToList();

        Defaults.AllFileNamesToSkip.ForEach(x =>
        {
            var webFile = webFiles.FirstOrDefault(y => x.Equals(y.Name, StringComparison.InvariantCultureIgnoreCase)); //TODO Add Apply/With/etc extensions
            if (webFile != null)
                webFile.IsSynchronizationEnabled = false;
        });

        return webFiles;
    }
}