namespace Auis.StackOverflow.Services.Utilities;

public class WindowsFileUtilityService : FileUtilityServiceBase, IFileUtilityService
{
    public async Task ExtractArchiveFileAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default)
    {
        await DownloadFileAsync(webFileInformation, cancellationToken);
        await ExecuteProcessAsync("C:\\Program Files\\7-Zip\\7z.exe", $"x \"{webFileInformation.TemporaryDownloadPath}\" -o\"{webFileInformation.ArchiveOutputDirectory}\"", cancellationToken);
    }
}