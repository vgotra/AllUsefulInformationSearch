namespace AllUsefulInformationSearch.StackOverflow.PostsParser.FileUtilityServices;

public class LinuxFileUtilityService : FileUtilityServiceBase, IFileUtilityService
{
    public Task DownloadFileAsync(string fileUri, string temporaryDownloadPath, CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    public Task UnarchiveFileAsync(string temporaryDownloadPath, string outputDirectory, CancellationToken cancellationToken = default) => 
        throw new NotImplementedException();
}