namespace Auis.StackOverflow.Services;

public class WindowsFileUtilityService(HttpClient httpClient) : FileUtilityServiceBase(httpClient), IFileUtilityService
{
    public async Task DownloadFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default) => 
        await DownloadFileAsync(webFilePaths.WebFileUri, webFilePaths.TemporaryDownloadPath, cancellationToken);

    //TODO Move to configuration
    public async Task ExtractArchiveFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default) => 
        await ExecuteProcessAsync("C:\\Program Files\\7-Zip\\7z.exe", $"x \"{webFilePaths.TemporaryDownloadPath}\" -o\"{webFilePaths.ArchiveOutputDirectory}\"", cancellationToken);
}