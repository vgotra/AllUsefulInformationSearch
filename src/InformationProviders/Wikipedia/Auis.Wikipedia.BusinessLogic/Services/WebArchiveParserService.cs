namespace Auis.Wikipedia.BusinessLogic.Services;

public sealed class WebArchiveParserService(HttpClient httpClient, IOptions<WikipediaOptions> options, ILogger<WebArchiveParserService> logger) : IWebArchiveParserService
{
    private static readonly Regex RegexItemsPattern =
        new("""<a href="(?<Link>[^"]+)">(?<Name>[^<]+)<\/a>\s*(?<LastModified>\d{2}-\w{3}-\d{4} \d{2}:\d{2})\s*(?<Size>[\d,]+)""", RegexOptions.Compiled);

    public async ValueTask<List<WebDataFile>> GetWebDataFilesAsync(CancellationToken cancellationToken = default)
    {
        var archiveHtmlPage = await httpClient.GetStringAsync(string.Empty, cancellationToken);
        var result = ParseLines(archiveHtmlPage);
        var files = result.Where(x => x.Link.StartsWith(options.Value.FileLinkStartsWith) && !x.Link.EndsWith(".xml")).ToList();
        return files;
    }

    private List<WebDataFile> ParseLines(string htmlText)
    {
        var result = new List<WebDataFile>();
        var matches = RegexItemsPattern.Matches(htmlText);
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

    private static WebDataFile ParseLine(Match match)
    {
        var link = match.Groups["Link"].Value;
        var name = link; //INFO: Name is not correct sometimes
        var lastModified = DateTime.Parse(match.Groups["LastModified"].Value);
        var size = match.Groups["Size"].Value.GetFileSize();
        return new WebDataFile { Name = name, Link = link, Size = size, LastModified = lastModified };
    }
}