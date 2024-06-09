using System.Text.RegularExpressions;

namespace Auis.StackOverflow.Services;

public class PostModificationService(ILogger<PostModificationService> logger) : IPostModificationService
{
    private static readonly Regex LiRegex = new(@"<li>", RegexOptions.Compiled);
    private static readonly Regex AllHtmlExceptLinks = new("<(?!a|/a).*?>", RegexOptions.Compiled);
    private static readonly Regex MultipleLineEnds = new("\n{2,}", RegexOptions.Compiled);
    private static readonly Regex MultipleSpaces = new(" {2,}", RegexOptions.Compiled);

    public async Task<List<PostModel>> PostProcessArchivePostsAsync(List<PostModel> posts, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting post processing of archive posts");
        foreach (var post in posts)
        {
            post.Body = CleanupText(post.Body);
            post.AcceptedAnswer!.Body = CleanupText(post.AcceptedAnswer.Body);
        }
        logger.LogInformation("Completed post processing of archive posts");
        await Task.CompletedTask;
        return posts;
    }

    private string CleanupText(string text)
    {
        var cleanedText = LiRegex.Replace(text, "- ");
        cleanedText = AllHtmlExceptLinks.Replace(cleanedText, string.Empty);
        cleanedText = MultipleLineEnds.Replace(cleanedText, "\n");
        cleanedText = MultipleSpaces.Replace(cleanedText, "\n");
        return cleanedText;
    }
}