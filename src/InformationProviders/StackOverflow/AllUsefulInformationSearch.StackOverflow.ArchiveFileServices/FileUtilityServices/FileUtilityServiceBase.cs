namespace AllUsefulInformationSearch.StackOverflow.PostsParser.FileUtilityServices;

public abstract class FileUtilityServiceBase(HttpClient httpClient)
{
    protected async Task DownloadFileAsync(string url, string filePath, CancellationToken cancellationToken = default)
    {
        await using var stream = await httpClient.GetStreamAsync(url, cancellationToken);
        await using var fileStream = File.Create(filePath);
        await stream.CopyToAsync(fileStream, cancellationToken);
    }

    protected async Task ExecuteProcessAsync(string fileName, string arguments, CancellationToken cancellationToken = default)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = false,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        process.Start();
        var errors = await process.StandardError.ReadToEndAsync(cancellationToken);
        await process.WaitForExitAsync(cancellationToken);
        if (process.ExitCode != 0)
            throw new IOException($"The process exited with code {process.ExitCode}. Errors: {errors}");
    }
}