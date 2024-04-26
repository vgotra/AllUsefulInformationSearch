namespace AllUsefulInformationSearch.StackOverflow.PostsParser.FileUtilityServices;

public interface IFileUtilityService
{
    Task DownloadFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default);
    
    Task ExtractArchiveFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default);

    void DeleteProcessedFiles(WebFilePaths webFilePaths)
    {
        if (File.Exists(webFilePaths.TemporaryDownloadPath))
            File.Delete(webFilePaths.TemporaryDownloadPath);
        if (Directory.Exists(webFilePaths.ArchiveOutputDirectory))
            Directory.Delete(webFilePaths.ArchiveOutputDirectory, true);
    }
}