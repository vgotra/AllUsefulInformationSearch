using System.Text.RegularExpressions;
using AllUsefulInformationSearch.Common.Http;
using AllUsefulInformationSearch.StackOverflow.Extensions;

namespace AllUsefulInformationSearch.StackOverflow;

public interface IStackOverflowArchiveParser
{
    Task<List<StackOverflowDataFile>> GetDataFileInfoListAsync(CancellationToken cancellationToken = default);
}

public class StackOverflowArchiveParser : IStackOverflowArchiveParser
{
    private const string StackOverflowArchiveUrl = "https://archive.org/download/stackexchange";

    private const string ItemsPattern =
        """<tr\s*>\s*<td>\s*<a href="(?<Link>[^<]*?7z[^<]*?)">(?<Name>[^<]*?7z[^<]*?)<\/a>.*<\/td>\s*<td>(?<LastModified>.*?)<\/td>\s*<td>(?<Size>.*?)<\/td>\s*<\/tr>""";

    private readonly IHttpClientFactoryWrapper _httpClientFactory;

    public StackOverflowArchiveParser(IHttpClientFactoryWrapper httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<StackOverflowDataFile>> GetDataFileInfoListAsync(
        CancellationToken cancellationToken = default)
    {
        var archiveHtmlPage = await DownloadPageAsync(cancellationToken);
        var result = ParseLines(archiveHtmlPage);
        return result;
    }

    private Task<string> DownloadPageAsync(CancellationToken cancellationToken = default) =>
        _httpClientFactory.CreateClient().GetStringAsync(StackOverflowArchiveUrl, cancellationToken);

    private List<StackOverflowDataFile> ParseLines(string htmlText)
    {
        var result = new List<StackOverflowDataFile>();
        var matches = Regex.Matches(htmlText, ItemsPattern, RegexOptions.Multiline);
        foreach (Match match in matches)
        {
            var item = ParseLine(match);
            result.Add(item);
        }

        return result;
    }

    private StackOverflowDataFile ParseLine(Match match)
    {
        var link = match.Groups["Link"].Value;
        var name = match.Groups["Name"].Value;
        var lastModified = DateTime.Parse(match.Groups["LastModified"].Value);
        var size = match.Groups["Size"].Value;
        var fileSize = size.GetFileSize();

        return new StackOverflowDataFile { Name = name, Link = link, Size = fileSize, LastModified = lastModified };
    }
}