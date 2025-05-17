using Auis.StackOverflow.Common.Helpers;

namespace Auis.StackOverflow.BusinessLogic.Services;

public sealed partial class WebArchiveParserService(HttpClient httpClient, ILogger<WebArchiveParserService> logger) : IWebArchiveParserService
{
    [GeneratedRegex("""<tr\s*>\s*<td>\s*<a href="(?<Link>[^<]*?7z[^<]*?)">(?<Name>[^<]*?7z[^<]*?)<\/a>.*<\/td>\s*<td>(?<LastModified>.*?)<\/td>\s*<td>(?<Size>[\d,\.]+[KMG]?)<\/td>\s*<\/tr>""", RegexOptions.Compiled)]
    private static partial Regex RegexItemsPattern();

    public async ValueTask<List<WebDataFile>> GetWebDataFilesAsync(CancellationToken cancellationToken = default)
    {
        var archiveHtmlPage = await httpClient.GetStringAsync(string.Empty, cancellationToken);
        var result = ParseLines(archiveHtmlPage);
        return result;
    }

    private List<WebDataFile> ParseLines(string htmlText)
    {
        var result = new List<WebDataFile>();
        var matches = RegexItemsPattern().Matches(htmlText);
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
        var name = match.Groups["Name"].Value;
        var lastModified = DateTime.Parse(match.Groups["LastModified"].Value);
        var size = match.Groups["Size"].Value.GetFileSize();
        return new WebDataFile { Name = name, Link = link, Size = size, LastModified = lastModified };
    }
}