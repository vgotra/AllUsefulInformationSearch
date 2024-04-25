namespace AllUsefulInformationSearch.StackOverflow.PostsParser.FileUtilityServices;

public class WindowsFileUtilityService : FileUtilityServiceBase, IFileUtilityService
{
    //TODO Test this and improve the error handling, also options for powershell and 7zip
    
    public async Task DownloadFileAsync(string fileUri, string temporaryDownloadPath, CancellationToken cancellationToken = default) => 
        await ExecuteProcessAsync("powershell.exe", $"-Command \"Invoke-WebRequest -Uri {fileUri} -OutFile {temporaryDownloadPath}\"", cancellationToken);

    public async Task UnarchiveFileAsync(string temporaryDownloadPath, string outputDirectory, CancellationToken cancellationToken = default) => 
        await ExecuteProcessAsync("7z.exe", $"x \"{temporaryDownloadPath}\" -o\"{outputDirectory}\"", cancellationToken);
}