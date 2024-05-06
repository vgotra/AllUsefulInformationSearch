namespace AllUsefulInformationSearch.StackOverflow.Services;

public class LinuxFileUtilityService : FileUtilityServiceBase, IFileUtilityService
{
    public async Task DownloadFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default)
    {
        var command = $"wget -O {webFilePaths.TemporaryDownloadPath} {StackOverflowBaseUri}{webFilePaths.WebFileUri}";
        await ExecuteProcessAsync("/bin/bash", $"-c \"{command}\"", cancellationToken);
    }

    public async Task ExtractArchiveFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default)
    {
        var command = $"7za x {webFilePaths.TemporaryDownloadPath} -o{webFilePaths.ArchiveOutputDirectory}";
        await ExecuteProcessAsync("/bin/bash", $"-c \"{command}\"", cancellationToken);
    }
}