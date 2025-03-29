using Downloader;

namespace Auis.StackOverflow.BusinessLogic.Utilities;

public abstract class FileUtilityServiceBase
{
    protected async Task DownloadFileAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default)
    {
        if (webFileInformation.FileLocation == FileLocation.Web)
            await DownloadFileAsync(webFileInformation.FileUri, webFileInformation.TemporaryDownloadPath, cancellationToken);
        else if (webFileInformation.FileLocation == FileLocation.Network)
            File.Copy(webFileInformation.FileUri, webFileInformation.TemporaryDownloadPath, true);
        else
            throw new ArgumentException("The file location is not supported.");
    }

    private async Task DownloadFileAsync(string url, string filePath, CancellationToken cancellationToken = default) =>
        await DownloadBuilder.New().WithConfiguration(new DownloadConfiguration
        {
            ChunkCount = Environment.ProcessorCount > 1 ? Environment.ProcessorCount / 2 : 1,
            ParallelDownload = true
        }).WithUrl(url).WithFileLocation(filePath).Build().StartAsync(cancellationToken);
}