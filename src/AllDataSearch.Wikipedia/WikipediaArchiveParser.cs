using System.Text.RegularExpressions;

namespace AllDataSearch.Wikipedia;

public interface IWikipediaArchiveParser
{
    Task<List<WikipediaDataFile>> GetDataFileInfoListAsync();
}

public class WikipediaArchiveParser : IWikipediaArchiveParser
{
    //TODO Improve it later to include files which you need
    private const string WikipediaArchiveUrl = "https://dumps.wikimedia.org/enwiki/latest/";
    private const string ItemsPattern = """<a href="(?<Link>[^<]*?gz)">(?<Name>[^<]*?gz)<\/a>\s*(?<LastModified>.* \d{2}:\d{2})\s*(?<Size>.*?)\s""";

    private readonly IHttpClientFactory _httpClientFactory;

    public WikipediaArchiveParser(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<WikipediaDataFile>> GetDataFileInfoListAsync()
    {
        var archiveHtmlPage = await DownloadPageAsync();
        var result = ParseLines(archiveHtmlPage);
        return result;
    }

    protected virtual Task<string> DownloadPageAsync() => _httpClientFactory.CreateClient().GetStringAsync(WikipediaArchiveUrl);

    private List<WikipediaDataFile> ParseLines(string htmlText)
    {
        var result = new List<WikipediaDataFile>();
        var matches = Regex.Matches(htmlText, ItemsPattern, RegexOptions.Multiline);
        foreach (Match match in matches)
        {
            var item = ParseLine(match);
            result.Add(item);
        }
        return result;
    }

    private WikipediaDataFile ParseLine(Match match)
    {
        var link = match.Groups["Link"].Value;
        var name = match.Groups["Name"].Value;
        var lastModified = DateTime.Parse(match.Groups["LastModified"].Value);
        var size = match.Groups["Size"].Value;

        return new WikipediaDataFile { Name = name, Link = link, Size = size, LastModified = lastModified };
    }
}