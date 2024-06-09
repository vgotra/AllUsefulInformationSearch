namespace Auis.StackOverflow.Services;

public interface IFileUtilityService
{
    Task ExtractArchiveFileAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default);

    void DeleteExtractedFiles(WebFileInformation webFileInformation)
    {
        if (Directory.Exists(webFileInformation.ArchiveOutputDirectory))
            Directory.Delete(webFileInformation.ArchiveOutputDirectory, true);
    }

    void DeleteTemporaryFiles(WebFileInformation webFileInformation)
    {
        if (File.Exists(webFileInformation.TemporaryDownloadPath))
            File.Delete(webFileInformation.TemporaryDownloadPath);
    }
}