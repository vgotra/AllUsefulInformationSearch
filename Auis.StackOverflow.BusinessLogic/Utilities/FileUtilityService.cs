namespace Auis.StackOverflow.BusinessLogic.Utilities;

public class FileUtilityService : FileUtilityServiceBase, IFileUtilityService
{
    public async Task ExtractArchiveFileAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default)
    {
        await DownloadFileAsync(webFileInformation, cancellationToken);
      
        ExtractFile(webFileInformation, cancellationToken);
    }
}