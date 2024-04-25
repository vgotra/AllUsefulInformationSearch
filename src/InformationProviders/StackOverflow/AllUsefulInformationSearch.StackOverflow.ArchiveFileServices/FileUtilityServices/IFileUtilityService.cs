namespace AllUsefulInformationSearch.StackOverflow.PostsParser.FileUtilityServices;

public interface IFileUtilityService
{
    Task DownloadFileAsync(string fileUri, string temporaryDownloadPath, CancellationToken cancellationToken = default);
    Task UnarchiveFileAsync(string temporaryDownloadPath, string outputDirectory, CancellationToken cancellationToken = default);
}