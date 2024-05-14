using System.Net.Http.Headers;

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
        
        await DownloadFileInChunksAsync(args[0], args[1]);
    }
    
    private static async Task DownloadFileInChunksAsync(string fileUrl, string filePath, CancellationToken cancellationToken = default)
    {
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Range = new RangeHeaderValue(0, 0);
        
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, fileUrl), cancellationToken);
        var contentRange = response.Content.Headers.ContentRange; //TODO Check content range 

        await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        var webStream = await httpClient.GetStreamAsync(fileUrl, cancellationToken);
        await webStream.CopyToAsync(fileStream, cancellationToken);
        
        //TODO Add support for downloading in chunks for faster download
    }
}