namespace AllUsefulInformationSearch.FileUtility;

static class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a URL to download the file from and path for saving file.");
            return;
        }
        
        await new FileDownloader().DownloadFileInChunksAsync(args[0], args[1]);
    }
}