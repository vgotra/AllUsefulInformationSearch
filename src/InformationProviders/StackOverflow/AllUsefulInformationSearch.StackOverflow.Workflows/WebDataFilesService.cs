namespace AllUsefulInformationSearch.StackOverflow.Workflows;

public class WebDataFilesService(StackOverflowDbContext dbContext, IWebArchiveParser parser, ILogger<WebDataFilesService> logger) : IWebDataFilesService
{
    public async Task SynchronizeWebDataFilesAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Started syncing web data files to database");
        
        var archiveFiles = await parser.GetFileInfoListAsync(cancellationToken);
        var archiveFileDict = archiveFiles.ToDictionary(x => x.Name, x => x);
        var dataFiles = await dbContext.WebDataFiles.ToListAsync(cancellationToken);
        var dataFileDict = dataFiles.ToDictionary(x => x.Name, x => x);

        var newFiles = archiveFileDict.Values.Where(x => !dataFileDict.ContainsKey(x.Name)).ToList();
        var updatedFiles = dataFileDict.Values.Where(x => archiveFileDict.ContainsKey(x.Name) && x.ExternalLastModified != archiveFileDict[x.Name].LastModified).ToList();
        // No need to delete some data files, because they can be useful

        if (newFiles.Count > 0)
            dbContext.WebDataFiles.AddRange(newFiles.Select(x => x.ToEntity()));

        if (updatedFiles.Count > 0)
        {
            updatedFiles.ForEach(x => x.ProcessingStatus = ProcessingStatus.Updated);
            dbContext.WebDataFiles.UpdateRange(updatedFiles);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Synchronized web data files to database");
    }
}