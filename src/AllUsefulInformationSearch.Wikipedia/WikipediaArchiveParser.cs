using System.Text.RegularExpressions;

namespace AllUsefulInformationSearch.Wikipedia;

public class WikipediaArchiveParser : IWikipediaArchiveParser
{
    private const string WikipediaArchiveUrl = "https://dumps.wikimedia.org/enwiki/latest/";

    private const string ItemsPattern = """<a href="(?<Link>[^<]*?gz)">(?<Name>[^<]*?gz)<\/a>\s*(?<LastModified>.* \d{2}:\d{2})\s*(?<Size>.*?)\s""";

    public async Task<List<WikipediaDataFile>> GetFileInfoListAsync(CancellationToken cancellationToken = default)
    {
        var archiveHtmlPage = await WikipediaArchiveUrl.GetStringAsync(cancellationToken: cancellationToken);
        return ParseLines(archiveHtmlPage);
    }

    private List<WikipediaDataFile> ParseLines(string htmlText)
    {
        var result = new List<WikipediaDataFile>();
        var matches = Regex.Matches(htmlText, ItemsPattern, RegexOptions.Multiline);
        foreach (Match match in matches)
            result.Add(ParseLine(match));
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