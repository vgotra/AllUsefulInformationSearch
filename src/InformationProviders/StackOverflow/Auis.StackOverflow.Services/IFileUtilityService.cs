namespace Auis.StackOverflow.Services;

public interface IFileUtilityService
{
    Task ExtractArchiveFileAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default);

    void DeleteProcessedFiles(WebFileInformation webFileInformation)
    {
        if (File.Exists(webFileInformation.TemporaryDownloadPath))
            File.Delete(webFileInformation.TemporaryDownloadPath);
        if (Directory.Exists(webFileInformation.ArchiveOutputDirectory))
            Directory.Delete(webFileInformation.ArchiveOutputDirectory, true);
    }
}