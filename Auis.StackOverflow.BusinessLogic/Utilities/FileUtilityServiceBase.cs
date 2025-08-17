using Downloader;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace Auis.StackOverflow.BusinessLogic.Utilities;

public abstract class FileUtilityServiceBase
{
    protected async Task DownloadFileAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default)
    {
        if (webFileInformation.FileLocation == FileLocation.Web)
            await DownloadWebFileAsync(webFileInformation, cancellationToken);
        else if (webFileInformation.FileLocation == FileLocation.Network)
            File.Copy(webFileInformation.FileUri, webFileInformation.TemporaryDownloadPath, true);
        else
            throw new ArgumentException("The file location is not supported.");
    }

    private static async Task DownloadWebFileAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default)
    {
        await using var ds = new DownloadService();
        await ds.DownloadFileTaskAsync(webFileInformation.FileUri, webFileInformation.TemporaryDownloadPath, cancellationToken);
        
        if (ds.Status != DownloadStatus.Completed)
            throw new InvalidOperationException($"Failed to download file from {webFileInformation.FileUri}. Status: {ds.Status}");
        
        if (!File.Exists(webFileInformation.TemporaryDownloadPath))
            throw new FileNotFoundException("The downloaded file does not exist.", webFileInformation.TemporaryDownloadPath);
    }

    protected static void ExtractFile(WebFileInformation webFileInformation, CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(webFileInformation.ArchiveOutputDirectory))
            Directory.CreateDirectory(webFileInformation.ArchiveOutputDirectory);
        
        var options = new ExtractionOptions { ExtractFullPath = true, Overwrite = true };
        using var archive = ArchiveFactory.Open(webFileInformation.TemporaryDownloadPath);
        foreach (var entry in archive.Entries.Where(e => !e.IsDirectory))
        {
            cancellationToken.ThrowIfCancellationRequested();
            entry.WriteToDirectory(webFileInformation.ArchiveOutputDirectory, options);
        }
    }
}