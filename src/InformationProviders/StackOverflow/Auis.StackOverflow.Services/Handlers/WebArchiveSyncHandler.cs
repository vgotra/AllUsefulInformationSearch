namespace Auis.StackOverflow.Services.Handlers;

public class WebArchiveSyncHandler(IServiceProvider serviceProvider, ILogger<WebArchiveSyncHandler> logger) : ICommandHandler<WebArchiveFilesSaveCommand>
{
    public async ValueTask<Unit> Handle(WebArchiveFilesSaveCommand command, CancellationToken cancellationToken)
    {
        var dbContext = serviceProvider.GetRequiredService<StackOverflowDbContext>();
        var dataFiles = await dbContext.WebDataFiles.ToListAsync(cancellationToken);

        var webFiles = GetWebDataFilesToSync(command.Files, dataFiles);
        if (webFiles.Count > 0)
        {
            await dbContext.AddRangeAsync(webFiles, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        logger.LogInformation("Web archive files synchronized");
        return Unit.Value;
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