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
        var contentRange = response.Content.Headers.ContentRange;
        var fileSize = (int)(contentRange?.Length ?? -1);
        if (fileSize is -1 or < 1024 * 1024)
        {
            await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write, 4096, true);
            var webStream = await httpClient.GetStreamAsync(fileUrl, cancellationToken);
            await webStream.CopyToAsync(fileStream, cancellationToken);
            return;
        }

        await using var chunksFileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write, 4096, true);
        var chunks = SplitFileIntoChunks(fileSize, 1024 * 1024);
        await Parallel.ForEachAsync(chunks, cancellationToken, async (range, token) =>
        {
            //TODO Check this later
            using var chunkHttpClient = new HttpClient(); //TODO Check for thread safety
            chunkHttpClient.DefaultRequestHeaders.Range = new RangeHeaderValue(range.From, range.To);
            var chunkResponse = await httpClient.GetAsync(fileUrl, token);
            var chunkBytes = await chunkResponse.Content.ReadAsByteArrayAsync(token);
            var offset = range.From == 0 ? 0 : range.From;
            await chunksFileStream.WriteAsync(chunkBytes, offset, chunkBytes.Length, token);
        });
    }
    
    private static List<(int From, int To)> SplitFileIntoChunks(long fileSize, int chunkSize)
    {
        var chunks = new List<(int From, int To)>();
        for (var i = 0; i < fileSize; i += chunkSize)
        {
            var from = i;
            var to = i + chunkSize - 1;
            if (to > fileSize - 1)
                to = (int)(fileSize - 1);
            chunks.Add((from, to));
        }
        return chunks;
    }
}