namespace AllUsefulInformationSearch.StackOverflow.PostsParser.FileUtilityServices;

public class LinuxFileUtilityService : FileUtilityServiceBase, IFileUtilityService
{
    public Task DownloadFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    public Task ExtractArchiveFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();
}