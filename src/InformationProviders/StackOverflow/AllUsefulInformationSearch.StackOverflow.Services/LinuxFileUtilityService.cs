namespace AllUsefulInformationSearch.StackOverflow.Services;

public class LinuxFileUtilityService(HttpClient httpClient) : FileUtilityServiceBase(httpClient), IFileUtilityService
{
    public async Task DownloadFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default) =>
        await DownloadFileAsync(webFilePaths.WebFileUri, webFilePaths.TemporaryDownloadPath, cancellationToken);

    public async Task ExtractArchiveFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default)
    {
        var command = $"7za x {webFilePaths.TemporaryDownloadPath} -o{webFilePaths.ArchiveOutputDirectory}";
        await ExecuteProcessAsync("/bin/bash", $"-c \"{command}\"", cancellationToken);
    }
}