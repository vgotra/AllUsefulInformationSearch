namespace Auis.StackOverflow.Models.Extensions;

public static class PostConversionExtensions
{
    public static PostEntity ToEntity(this PostModel post) =>
        new()
        {
            Id = post.Id,
            WebDataFileId = post.WebDataFileId,
            Title = post.Title,
            Question = post.Body,
            Answer = post.AcceptedAnswer.Body,
            QuestionExternalLastActivityDate = post.LastActivityDate,
            AnswerExternalLastActivityDate = post.AcceptedAnswer.LastActivityDate
        };
}