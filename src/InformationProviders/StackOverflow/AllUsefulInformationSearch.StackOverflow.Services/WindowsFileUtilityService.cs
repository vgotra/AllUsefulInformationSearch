namespace AllUsefulInformationSearch.StackOverflow.Services;

public class WindowsFileUtilityService(HttpClient httpClient) : FileUtilityServiceBase(httpClient), IFileUtilityService
{
    public async Task DownloadFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default) => 
        await DownloadFileAsync(webFilePaths.WebFileUri, webFilePaths.TemporaryDownloadPath, cancellationToken);

    public async Task ExtractArchiveFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default) => 
        await ExecuteProcessAsync("7z.exe", $"x \"{webFilePaths.TemporaryDownloadPath}\" -o\"{webFilePaths.ArchiveOutputDirectory}\"", cancellationToken);
}