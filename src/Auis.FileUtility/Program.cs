using Downloader;
using System.Diagnostics;

namespace Auis.FileUtility;

static class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a URL to download the file from and path for saving file.");
            return;
        }

        var sw = Stopwatch.StartNew();
        Console.WriteLine($"Started downloading file '{args[0]}' to '{args[1]}'...");
        await DownloadBuilder.New().WithConfiguration(new DownloadConfiguration
        {
            ChunkCount = Environment.ProcessorCount > 1 ? Environment.ProcessorCount / 2 : 1,
            ParallelDownload = true
        }).WithUrl(args[0]).WithFileLocation(args[1]).Build().StartAsync();
        Console.WriteLine($"Completed downloading file '{args[0]}'. Time taken: {sw.Elapsed}.");
    }
}