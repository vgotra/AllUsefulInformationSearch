using Auis.Common.Helpers;
using Auis.StackOverflow.BusinessLogic.Helpers;

namespace Auis.StackOverflow.BusinessLogic.Services;

public class PostsArchiveFileParserService(IPostTextCleanupService postTextCleanupService) : IPostsArchiveFileParserService
{
    private const string PostTypeQuestion = "1";
    private const string PostTypeAnswer = "2";
    
    public async Task<List<PostEntity>> DeserializePostsAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default)
    {
        var pathToPostsFile = Path.Combine(webFileInformation.ArchiveOutputDirectory, "Posts.xml");

        var questions = new List<PostEntity>((int)(webFileInformation.FileSize / FileSize.Mb * 100));
        var answers = new Dictionary<int, PostEntity>((int)(webFileInformation.FileSize / FileSize.Mb * 100));

        using var streamReader = new StreamReader(pathToPostsFile, new FileStreamOptions { Options = FileOptions.Asynchronous });
        await streamReader.ReadLineAsync(cancellationToken); // <xml>
        await streamReader.ReadLineAsync(cancellationToken); // </posts>
        while (await streamReader.ReadLineAsync(cancellationToken) is { } line && !cancellationToken.IsCancellationRequested)
            ParseLine(line, questions, answers);

        questions = questions.Where(x => answers.ContainsKey(x.AcceptedAnswerId)).ToList(); // We only want questions with accepted answers
        questions.ForEach(x =>
        {
            var answer = answers[x.AcceptedAnswerId];
            x.WebDataFileId = webFileInformation.WebDataFileId;
            x.Title = postTextCleanupService.CleanupHtmlText(x.Title);
            x.Question = postTextCleanupService.CleanupHtmlText(x.Question);
            x.Answer = postTextCleanupService.CleanupHtmlText(answer.Answer);
            x.AnswerExternalLastActivityDate = answer.AnswerExternalLastActivityDate;
        });

        return questions;
    }

    private static void ParseLine(string line, List<PostEntity> questions, Dictionary<int, PostEntity> answers)
    {
        var postType = line.GetValue("PostTypeId");
        if (postType.IsEmpty) return;
        if (postType is PostTypeQuestion)
        {
            var question = ParseQuestionEntity(line);
            if (question != null)
                questions.Add(question);
        }
        else if (postType is PostTypeAnswer)
        {
            var answer = ParseAnswerEntity(line);
            answers.Add(answer.Id, answer);
        }
    }

    private static PostEntity? ParseQuestionEntity(string line)
    {
        var titleSpan = line.GetValue("Title");
        var bodySpan = line.GetValue("Body");
        var acceptedAnswerIdSpan = line.GetValue("AcceptedAnswerId");
        if (acceptedAnswerIdSpan.IsEmpty) return null;

        return new PostEntity
        {
            Id = int.Parse(line.GetValue("Id")),
            Title = !titleSpan.IsEmpty ? titleSpan.ToString() : string.Empty,
            Question = !bodySpan.IsEmpty ? bodySpan.ToString() : string.Empty,
            QuestionExternalLastActivityDate = DateTimeOffset.Parse(line.GetValue("LastActivityDate")),
            AcceptedAnswerId = int.Parse(acceptedAnswerIdSpan)
        };
    }

    private static PostEntity ParseAnswerEntity(string line)
    {
        var bodySpan = line.GetValue("Body");
        return new PostEntity
        {
            Id = int.Parse(line.GetValue("Id")),
            Answer = !bodySpan.IsEmpty ? bodySpan.ToString() : string.Empty,
            AnswerExternalLastActivityDate = DateTimeOffset.Parse(line.GetValue("LastActivityDate"))
        };
    }
}