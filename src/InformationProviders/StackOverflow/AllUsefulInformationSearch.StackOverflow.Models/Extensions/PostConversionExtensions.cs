namespace AllUsefulInformationSearch.StackOverflow.Models.Extensions;

public static class PostConversionExtensions
{
    public static PostEntity ToEntity(this Post post) =>
        new()
        {
            Id = post.Id,
            Title = post.Title,
            Text = post.Body,
            Tags = post.Tags,
            ExternalLastActivityDate = post.LastActivityDate,
            AcceptedAnswer = post.AcceptedAnswer?.ToEntity(post.Id),
            WebDataFileId = post.WebDataFileId
        };
    
    private static AcceptedAnswerEntity ToEntity(this Post post, int postId) =>
        new()
        {
            Id = post.Id,
            Text = post.Body,
            ExternalLastActivityDate = post.LastActivityDate,
            PostId = postId,
            PostWebDataFileId = post.WebDataFileId
        };
}