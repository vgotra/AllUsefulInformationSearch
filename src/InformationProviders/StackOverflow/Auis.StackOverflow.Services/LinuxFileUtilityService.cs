namespace Auis.StackOverflow.Services;

public class LinuxFileUtilityService : FileUtilityServiceBase, IFileUtilityService
{
    public async Task ExtractArchiveFileAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default)
    {
        await DownloadFileAsync(webFileInformation, cancellationToken);
        var command = $"7za x {webFileInformation.TemporaryDownloadPath} -o{webFileInformation.ArchiveOutputDirectory}";
        await ExecuteProcessAsync("/bin/bash", $"-c \"{command}\"", cancellationToken);
    }
}