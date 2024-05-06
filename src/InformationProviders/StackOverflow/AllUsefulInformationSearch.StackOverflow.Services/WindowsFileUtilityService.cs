namespace AllUsefulInformationSearch.StackOverflow.Services;

public class WindowsFileUtilityService : FileUtilityServiceBase, IFileUtilityService
{
    public async Task DownloadFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default) => 
        await ExecuteProcessAsync("powershell.exe", $"-Command \"Invoke-WebRequest -Uri {StackOverflowBaseUri}{webFilePaths.WebFileUri} -OutFile {webFilePaths.TemporaryDownloadPath}\"", cancellationToken);

    public async Task ExtractArchiveFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default) => 
        await ExecuteProcessAsync("7z.exe", $"x \"{webFilePaths.TemporaryDownloadPath}\" -o\"{webFilePaths.ArchiveOutputDirectory}\"", cancellationToken);
}