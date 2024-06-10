namespace Auis.StackOverflow.Services;

public sealed class WebArchiveParserHandler(HttpClient httpClient, ILogger<WebArchiveParserHandler> logger) : IQueryHandler<WebArchiveParserQuery, WebArchiveParserResponse>
{
    private const string ItemsPattern = """<tr\s*>\s*<td>\s*<a href="(?<Link>[^<]*?7z[^<]*?)">(?<Name>[^<]*?7z[^<]*?)<\/a>.*<\/td>\s*<td>(?<LastModified>.*?)<\/td>\s*<td>(?<Size>[\d,\.]+[KMG]?)<\/td>\s*<\/tr>""";

    public async ValueTask<WebArchiveParserResponse> Handle(WebArchiveParserQuery query, CancellationToken cancellationToken)
    {
        var archiveHtmlPage = await httpClient.GetStringAsync(string.Empty, cancellationToken);
        var result = ParseLines(archiveHtmlPage);

        return new WebArchiveParserResponse(result);
    }

    private List<StackOverflowDataFile> ParseLines(string htmlText)
    {
        var result = new List<StackOverflowDataFile>();
        var matches = Regex.Matches(htmlText, ItemsPattern, RegexOptions.Multiline);
        foreach (Match match in matches)
        {
            try
            {
                result.Add(ParseLine(match));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Cannot parse line data. Line raw data: {LineRawData}", match.Value);
            }
        }

        return result;
    }

    private static StackOverflowDataFile ParseLine(Match match)
    {
        var link = match.Groups["Link"].Value;
        var name = match.Groups["Name"].Value;
        var lastModified = DateTime.Parse(match.Groups["LastModified"].Value);
        var size = match.Groups["Size"].Value.GetFileSize();
        return new StackOverflowDataFile { Name = name, Link = link, Size = size, LastModified = lastModified };
    }
}