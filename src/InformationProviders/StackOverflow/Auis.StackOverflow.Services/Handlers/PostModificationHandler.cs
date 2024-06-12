namespace Auis.StackOverflow.Services.Handlers;

public class PostModificationHandler : IRequestHandler<PostModificationRequest, PostModificationResponse>
{
    //TODO Move this to parsing later to minimize memory consumption and performance
    private static readonly Regex LiRegex = new(@"<li>", RegexOptions.Compiled);
    private static readonly Regex AllHtmlExceptLinks = new("<(?!a|/a).*?>", RegexOptions.Compiled);
    private static readonly Regex MultipleLineEnds = new("\n{2,}", RegexOptions.Compiled);
    private static readonly Regex MultipleSpaces = new(" {2,}", RegexOptions.Compiled);

    private string CleanupText(string text)
    {
        var cleanedText = LiRegex.Replace(text, "- ");
        cleanedText = AllHtmlExceptLinks.Replace(cleanedText, string.Empty);
        cleanedText = MultipleLineEnds.Replace(cleanedText, "\n");
        cleanedText = MultipleSpaces.Replace(cleanedText, "\n");
        return cleanedText;
    }

    public ValueTask<PostModificationResponse> Handle(PostModificationRequest request, CancellationToken cancellationToken)
    {
        foreach (var post in request.Posts)
        {
            post.Body = CleanupText(post.Body);
            post.AcceptedAnswer!.Body = CleanupText(post.AcceptedAnswer.Body);
        }

        return new ValueTask<PostModificationResponse>(new PostModificationResponse(request.Posts));
    }
}

