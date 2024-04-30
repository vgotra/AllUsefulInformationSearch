using AllUsefulInformationSearch.StackOverflow.DataAccess.Entities;
using AllUsefulInformationSearch.StackOverflow.PostsParser.XmlModels;

namespace AllUsefulInformationSearch.StackOverflow.Workflows.Extensions;

public static class PostConversionExtensions
{
    public static PostEntity ToEntity(this Post post, Guid webDataFileId) =>
        new()
        {
            Id = post.Id,
            Title = post.Title,
            Text = post.Body,
            Tags = post.Tags,
            ExternalCreationDate = post.CreationDate,
            ExternalLastActivityDate = post.LastActivityDate,
            LastUpdated = DateTimeOffset.UtcNow,
            AcceptedAnswer = post.AcceptedAnswer?.ToEntity(post.Id, webDataFileId),
            WebDataFileId = webDataFileId
        };
    
    public static AcceptedAnswerEntity ToEntity(this Post post, int postId, Guid webDataFileId) =>
        new()
        {
            Id = post.Id,
            Text = post.Body,
            ExternalCreationDate = post.CreationDate,
            ExternalLastActivityDate = post.LastActivityDate,
            LastUpdated = DateTimeOffset.UtcNow,
            PostId = postId,
            PostWebDataFileId = webDataFileId
        };
}