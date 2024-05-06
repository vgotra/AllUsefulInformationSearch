namespace AllUsefulInformationSearch.StackOverflow.Models.Extensions;

public static class PostConversionExtensions
{
    public static PostEntity ToEntity(this Post post, int webDataFileId) =>
        new()
        {
            Id = post.Id,
            Title = post.Title,
            Text = post.Body,
            Tags = post.Tags,
            ExternalLastActivityDate = post.LastActivityDate,
            LastUpdated = DateTimeOffset.UtcNow,
            AcceptedAnswer = post.AcceptedAnswer?.ToEntity(post.Id, webDataFileId),
            WebDataFileId = webDataFileId
        };
    
    public static AcceptedAnswerEntity ToEntity(this Post post, int postId, int webDataFileId) =>
        new()
        {
            Id = post.Id,
            Text = post.Body,
            ExternalLastActivityDate = post.LastActivityDate,
            LastUpdated = DateTimeOffset.UtcNow,
            PostId = postId,
            PostWebDataFileId = webDataFileId
        };
}