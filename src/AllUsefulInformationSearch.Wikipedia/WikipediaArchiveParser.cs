using System.Text.RegularExpressions;
using AllUsefulInformationSearch.Common.Http;

namespace AllUsefulInformationSearch.Wikipedia;

public interface IWikipediaArchiveParser
{
    Task<List<WikipediaDataFile>> GetDataFileInfoListAsync(CancellationToken cancellationToken = default);
}

public class WikipediaArchiveParser : IWikipediaArchiveParser
{
    //TODO Improve it later to include files which you need
    private const string WikipediaArchiveUrl = "https://dumps.wikimedia.org/enwiki/latest/";

    private const string ItemsPattern =
        """<a href="(?<Link>[^<]*?gz)">(?<Name>[^<]*?gz)<\/a>\s*(?<LastModified>.* \d{2}:\d{2})\s*(?<Size>.*?)\s""";

    private readonly IHttpClientFactoryWrapper _httpClientFactory;

    public WikipediaArchiveParser(IHttpClientFactoryWrapper httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<WikipediaDataFile>> GetDataFileInfoListAsync(CancellationToken cancellationToken = default)
    {
        var archiveHtmlPage = await DownloadPageAsync(cancellationToken);
        var result = ParseLines(archiveHtmlPage);
        return result;
    }

    private Task<string> DownloadPageAsync(CancellationToken cancellationToken = default) =>
        _httpClientFactory.CreateClient().GetStringAsync(WikipediaArchiveUrl, cancellationToken);

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
        var size = long.Parse(match.Groups["Size"].Value);

        return new WikipediaDataFile { Name = name, Link = link, Size = size, LastModified = lastModified };
    }
}