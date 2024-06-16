namespace Auis.StackOverflow.BusinessLogic.Services;

public class PostTextCleanupService : IPostTextCleanupService
{
    //TODO Find how to cleanup ReadOnlySpan<char> instead of string
    private static readonly Regex LiRegex = new("<li>", RegexOptions.Compiled);
    private static readonly Regex AllHtmlExceptLinks = new("<(?!a|/a).*?>", RegexOptions.Compiled);
    private static readonly Regex MultipleLineEnds = new("\n{2,}", RegexOptions.Compiled);
    private static readonly Regex MultipleSpaces = new(" {2,}", RegexOptions.Compiled);

    public string CleanupHtmlText(string text)
    {
        var cleanedText = LiRegex.Replace(text, "- ");
        cleanedText = AllHtmlExceptLinks.Replace(cleanedText, string.Empty);
        cleanedText = MultipleLineEnds.Replace(cleanedText, "\n");
        cleanedText = MultipleSpaces.Replace(cleanedText, "\n");
        return cleanedText;
    }
}