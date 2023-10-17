using System.Text.RegularExpressions;

namespace AllDataSearch.StackOverflow;

public interface IStackOverflowArchiveParser
{
    Task<List<StackOverflowDataFile>> GetDataFileInfoListAsync();
}

public class StackOverflowArchiveParser : IStackOverflowArchiveParser
{
    private const string StackOverflowArchiveUrl = "https://archive.org/download/stackexchange";
    private const string ItemsPattern = """<tr\s*>\s*<td>\s*<a href="(?<Link>[^<]*?7z[^<]*?)">(?<Name>[^<]*?7z[^<]*?)<\/a>.*<\/td>\s*<td>(?<LastModified>.*?)<\/td>\s*<td>(?<Size>.*?)<\/td>\s*<\/tr>""";

    private readonly IHttpClientFactory _httpClientFactory;

    public StackOverflowArchiveParser(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<StackOverflowDataFile>> GetDataFileInfoListAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var archiveHtmlPage = await httpClient.GetStringAsync(StackOverflowArchiveUrl);
        var result = ParseLines(archiveHtmlPage);
        return result;
    }

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

        return new StackOverflowDataFile { Name = name, Link = link, Size = size, LastModified = lastModified };
    }
}