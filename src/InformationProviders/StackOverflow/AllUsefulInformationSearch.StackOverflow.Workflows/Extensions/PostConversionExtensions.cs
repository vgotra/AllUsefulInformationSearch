using AllUsefulInformationSearch.StackOverflow.DataAccess.Entities;
using AllUsefulInformationSearch.StackOverflow.PostsParser.XmlModels;

namespace AllUsefulInformationSearch.StackOverflow.Workflows.Extensions;

public static class PostConversionExtensions
{
    public static PostEntity ToEntity(this Post post) =>
        new()
        {
            Id = post.Id,
            Title = post.Title,
            Text = post.Body,
            ExternalCreationDate = post.CreationDate,
            Tags = post.Tags,
            LastUpdated = DateTimeOffset.UtcNow,
            AcceptedAnswer = post.AcceptedAnswer?.ToEntity(post.Id)
        };
    
    public static AcceptedAnswerEntity ToEntity(this Post post, int postId) =>
        new()
        {
            Id = post.Id,
            Title = post.Title,
            Text = post.Body,
            ExternalCreationDate = post.CreationDate,
            Tags = post.Tags,
            LastUpdated = DateTimeOffset.UtcNow,
            PostId = postId
        };
}