using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace AllUsefulInformationSearch.FileUtility;

public class FileDownloader(bool isChunksDownloadingEnabled = true)
{
    public async Task DownloadFileInChunksAsync(string fileUrl, string filePath, CancellationToken cancellationToken = default)
    {
        var rangeCheck = await SupportsRangeHeader(fileUrl);
        if (!isChunksDownloadingEnabled || !rangeCheck.IsRangeSupported)
        {
            var sw = Stopwatch.StartNew();
            await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write, 4096, true);
            var webStream = await new HttpClient().GetStreamAsync(fileUrl, cancellationToken);
            await webStream.CopyToAsync(fileStream, cancellationToken);
            Console.WriteLine($"File downloaded for {sw.Elapsed}");
            return;
        }

        var chunks = SplitFileIntoChunks(rangeCheck.FileSize, 1024 * 1024 * 10);
        var streams = new ConcurrentBag<(int Index, Stream Stream)>();
        var swp = Stopwatch.StartNew();
        await Parallel.ForEachAsync(chunks, cancellationToken, async (range, token) =>
        {
            using var chunkHttpClient = new HttpClient();
            chunkHttpClient.DefaultRequestHeaders.Range = new RangeHeaderValue(range.From, range.To);
            var chunkStream = await chunkHttpClient.GetStreamAsync(fileUrl, token);
            streams.Add((range.Index, chunkStream));
        });
        
        //TODO Check preallocation later also buffer size, chunkSizes, etc
        await using var chunksFileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write, 81920, true);
        streams.OrderBy(x => x.Index).ToList().ForEach(x =>
        {
            x.Stream.CopyTo(chunksFileStream);
            x.Stream.Dispose();
        });
        Console.WriteLine($"File chunks downloaded for {swp.Elapsed}");
    }
    
    private List<(int Index, int From, int To)> SplitFileIntoChunks(long fileSize, int chunkSize)
    {
        var chunks = new List<(int Index, int From, int To)>();
        var index = 0;
        for (var i = 0; i < fileSize; i += chunkSize)
        {
            var from = i;
            var to = i + chunkSize - 1;
            if (to > fileSize - 1)
                to = (int)(fileSize - 1);
            chunks.Add((index, from, to));
            index++;
        }
        return chunks;
    }
    
    private async Task<(bool IsRangeSupported, long FileSize)> SupportsRangeHeader(string url)
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
        return (response.Headers.AcceptRanges.Contains("bytes"), response.Content.Headers.ContentLength ?? 0);
    }
}