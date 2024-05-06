using System.Diagnostics;

namespace AllUsefulInformationSearch.StackOverflow.Services;

public abstract class FileUtilityServiceBase
{
    protected const string StackOverflowBaseUri = "https://archive.org/download/stackexchange/";//TODO Move to options later 
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