using System.Text.RegularExpressions;
using AllUsefulInformationSearch.Common.Http;
using AllUsefulInformationSearch.StackOverflow.Extensions;

namespace AllUsefulInformationSearch.StackOverflow;

public class StackOverflowArchiveParser : IStackOverflowArchiveParser
{
    private const string StackOverflowArchiveUrl = "https://archive.org/download/stackexchange";

    private const string ItemsPattern = """<tr\s*>\s*<td>\s*<a href="(?<Link>[^<]*?7z[^<]*?)">(?<Name>[^<]*?7z[^<]*?)<\/a>.*<\/td>\s*<td>(?<LastModified>.*?)<\/td>\s*<td>(?<Size>.*?)<\/td>\s*<\/tr>""";

    private readonly IHttpClientFactoryWrapper _httpClientFactory;

    public StackOverflowArchiveParser(IHttpClientFactoryWrapper httpClientFactory) => _httpClientFactory = httpClientFactory;

    public async Task<List<StackOverflowDataFile>> GetFileInfoListAsync(CancellationToken cancellationToken = default)
    {
        var archiveHtmlPage = await DownloadPageAsync(cancellationToken);
        return ParseLines(archiveHtmlPage);
    }

    private Task<string> DownloadPageAsync(CancellationToken cancellationToken = default) => _httpClientFactory.CreateClient().GetStringAsync(StackOverflowArchiveUrl, cancellationToken);

    private List<StackOverflowDataFile> ParseLines(string htmlText)
    {
        var result = new List<StackOverflowDataFile>();
        var matches = Regex.Matches(htmlText, ItemsPattern, RegexOptions.Multiline);
        foreach (Match match in matches)
            result.Add(ParseLine(match));
        return result;
    }

    private StackOverflowDataFile ParseLine(Match match)
    {
        var link = match.Groups["Link"].Value;
        var name = match.Groups["Name"].Value;
        var lastModified = DateTime.Parse(match.Groups["LastModified"].Value);
        var size = match.Groups["Size"].Value.GetFileSize();
        return new StackOverflowDataFile { Name = name, Link = link, Size = size, LastModified = lastModified };
    }
}