namespace Auis.StackOverflow.Services.Extensions;

public static class ProcessExtensions
{
    public static async Task ExecuteProcessAsync(this string fileName, string arguments, CancellationToken cancellationToken = default)
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

        var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);

        await process.WaitForExitAsync(cancellationToken);

        var errors = await errorTask;
        if (process.ExitCode != 0)
            throw new IOException($"The process exited with code {process.ExitCode}. Errors: {errors}");
    }
}