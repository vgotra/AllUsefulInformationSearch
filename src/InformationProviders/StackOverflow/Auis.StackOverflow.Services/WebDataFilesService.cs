namespace Auis.StackOverflow.Services;

public class WebDataFilesService(StackOverflowDbContext dbContext, IWebArchiveParserService parserService, ILogger<WebDataFilesService> logger) : IWebDataFilesService
{
    // Block all russian information because useless
    private readonly string[] _additionalFileNamesToSkip =
    [
        "ru.meta.stackoverflow.com.7z",
        "ru.stackoverflow.com.7z",
        "rus.meta.stackexchange.com.7z",
        "rus.stackexchange.com.7z",
        "russian.meta.stackexchange.com.7z",
        "russian.stackexchange.com.7z"
    ];

    //TODO Move later to settings
    private readonly string[] _fileNamesToSkip =
    [
        "stackoverflow.com-Badges.7z",
        "stackoverflow.com-PostLinks.7z",
        "stackoverflow.com-Tags.7z",
        "stackoverflow.com-Users.7z",
        "stackoverflow.com-Comments.7z",
        "stackoverflow.com-Votes.7z",
        "stackoverflow.com-PostHistory.7z",
        "stackoverflow.com-Posts.7z"
    ];

    public async Task SynchronizeWebDataFilesAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Started syncing web data files to database");

        var archiveFiles = await parserService.GetFileInfoListAsync(cancellationToken);
        var archiveFileDict = archiveFiles.ToDictionary(x => x.Name, x => x);
        var dataFiles = await dbContext.WebDataFiles.ToListAsync(cancellationToken);
        var dataFileDict = dataFiles.ToDictionary(x => x.Name, x => x);

        var newFiles = archiveFileDict.Values.Where(x => !dataFileDict.ContainsKey(x.Name));
        var updatedFiles = dataFileDict.Values.Where(x => archiveFileDict.ContainsKey(x.Name) && x.ExternalLastModified != archiveFileDict[x.Name].LastModified).ToList();
        // No need to delete some data files, because they can be useful

        updatedFiles.ForEach(x => x.ProcessingStatus = ProcessingStatus.Updated);
        var webFiles = newFiles.Select(x => x.ToEntity()).Concat(updatedFiles).ToList();

        foreach (var fileToSkip in _fileNamesToSkip.Concat(_additionalFileNamesToSkip))
            webFiles.First(x => x.Name == fileToSkip).IsSynchronizationEnabled = false;

        await dbContext.AddRangeAsync(webFiles, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Synchronized web data files to database");
    }
}