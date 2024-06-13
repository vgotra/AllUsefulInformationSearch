namespace Auis.StackOverflow.Services.Utilities;

public class WindowsFileUtilityService : FileUtilityServiceBase, IFileUtilityService
{
    public async Task ExtractArchiveFileAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default)
    {
        await DownloadFileAsync(webFileInformation, cancellationToken);
        await "C:\\Program Files\\7-Zip\\7z.exe".ExecuteProcessAsync($"x \"{webFileInformation.TemporaryDownloadPath}\" -o\"{webFileInformation.ArchiveOutputDirectory}\"", cancellationToken);
    }
}