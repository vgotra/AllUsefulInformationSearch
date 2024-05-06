namespace AllUsefulInformationSearch.StackOverflow.Models.Extensions;

public static class PostConversionExtensions
{
    public static PostEntity ToEntity(this PostModel post) =>
        new()
        {
            Id = post.Id,
            Title = post.Title,
            Text = post.Body,
            ExternalLastActivityDate = post.LastActivityDate,
            AcceptedAnswer = post.AcceptedAnswer!.ToEntity(post.Id), // we save only answered posts
            WebDataFileId = post.WebDataFileId
        };

    private static AcceptedAnswerEntity ToEntity(this PostModel post, int postId) =>
        new()
        {
            Id = post.Id,
            Text = post.Body,
            ExternalLastActivityDate = post.LastActivityDate,
            PostId = postId,
            PostWebDataFileId = post.WebDataFileId
        };
}