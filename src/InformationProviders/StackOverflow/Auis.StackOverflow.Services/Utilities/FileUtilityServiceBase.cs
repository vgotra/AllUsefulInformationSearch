using Downloader;

namespace Auis.StackOverflow.Services.Utilities;

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

    protected async Task ExecuteProcessAsync(string fileName, string arguments, CancellationToken cancellationToken = default)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = false,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        process.Start();
        var errors = await process.StandardError.ReadToEndAsync(cancellationToken);
        await process.WaitForExitAsync(cancellationToken);
        if (process.ExitCode != 0)
            throw new IOException($"The process exited with code {process.ExitCode}. Errors: {errors}");
    }
}