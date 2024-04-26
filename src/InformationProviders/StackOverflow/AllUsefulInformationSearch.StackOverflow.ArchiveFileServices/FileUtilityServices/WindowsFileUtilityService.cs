namespace AllUsefulInformationSearch.StackOverflow.PostsParser.FileUtilityServices;

public class WindowsFileUtilityService : FileUtilityServiceBase, IFileUtilityService
{
    //TODO Test this and improve the error handling, also options for powershell and 7zip
    
    public async Task DownloadFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default) => 
        await ExecuteProcessAsync("powershell.exe", $"-Command \"Invoke-WebRequest -Uri {webFilePaths.WebFileUri} -OutFile {webFilePaths.TemporaryDownloadPath}\"", cancellationToken);

    public async Task ExtractArchiveFileAsync(WebFilePaths webFilePaths, CancellationToken cancellationToken = default) => 
        await ExecuteProcessAsync("7z.exe", $"x \"{webFilePaths.TemporaryDownloadPath}\" -o\"{webFilePaths.ArchiveOutputDirectory}\"", cancellationToken);
}