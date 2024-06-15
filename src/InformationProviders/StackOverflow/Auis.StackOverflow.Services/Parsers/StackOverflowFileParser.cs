namespace Auis.StackOverflow.Services.Parsers;

public static class StackOverflowFileParser
{
    public static async Task<List<PostEntity>> DeserializePostsAsync(this string filePath, int webDataFileId, long fileSize, CancellationToken cancellationToken = default)
    {
        var questions = new List<PostEntity>((int)(fileSize / FileSize.Mb * 100));
        var answers = new Dictionary<int, PostEntity>((int)(fileSize / FileSize.Mb * 100));

        using var streamReader = new StreamReader(filePath, new FileStreamOptions { Options = FileOptions.Asynchronous });
        await streamReader.ReadLineAsync(cancellationToken); // <xml>
        await streamReader.ReadLineAsync(cancellationToken); // </posts>
        while (!streamReader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await streamReader.ReadLineAsync(cancellationToken);
            ParseLine(line!, questions, answers);
        }

        questions.ForEach(x =>
        {
            var answer = answers[x.AcceptedAnswerId];
            x.WebDataFileId = webDataFileId;
            x.Answer = answer.Answer;
            x.AnswerExternalLastActivityDate = answer.AnswerExternalLastActivityDate;
        });

        return questions;
    }

    private static ReadOnlySpan<char> GetValue(this string line, string attributeName)
    {
        var start = line.IndexOf(attributeName + "=\"", StringComparison.OrdinalIgnoreCase);
        if (start == -1)
            return default;
        start += attributeName.Length + 2;
        var end = line.IndexOf('"', start);
        return line.AsSpan().Slice(start, end - start);
    }

    private static void ParseLine(this string line, List<PostEntity> questions, Dictionary<int, PostEntity> answers)
    {
        var postType = line.GetValue("PostTypeId");
        if (postType.IsEmpty) return;
        if (postType == "1") //question
        {
            var question = line.ParseQuestionEntity();
            if (question != null)
                questions.Add(question);
        }
        else if (postType == "2") //answer
        {
            var answer = line.ParseAnswerEntity();
            answers.Add(answer.Id, answer);
        }
    }

    private static PostEntity? ParseQuestionEntity(this string line)
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

    private static PostEntity ParseAnswerEntity(this string line)
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