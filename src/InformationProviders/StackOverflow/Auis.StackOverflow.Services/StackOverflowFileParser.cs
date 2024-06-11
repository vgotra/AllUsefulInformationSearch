namespace Auis.StackOverflow.Services;

public static class StackOverflowFileParser
{
    public static async Task<List<PostModel>> DeserializePostsAsync(this string filePath, long fileSize, CancellationToken cancellationToken = default)
    {
        var list = new List<PostModel>(fileSize.GetApproximateItemsCount());
        using var streamReader = new StreamReader(filePath);

        // skip 2 lines for xml files
        await streamReader.ReadLineAsync(cancellationToken);
        await streamReader.ReadLineAsync(cancellationToken);

        while (!streamReader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await streamReader.ReadLineAsync(cancellationToken);
            if (line.IsXmlRow())
                list.Add(line!.ParsePostXmlRowLine());
        }

        return list;
    }

    private static bool IsXmlRow(this string? line) => line?.IndexOf("<row", StringComparison.InvariantCultureIgnoreCase) != -1;

    private static int GetApproximateItemsCount(this long fileSize) => (int)(fileSize / FileSize.Mb * 100);

    private static ReadOnlySpan<char> GetValue(this string line, string attributeName)
    {
        var start = line.IndexOf(attributeName + "=\"", StringComparison.OrdinalIgnoreCase);
        if (start == -1)
            return default;
        start += attributeName.Length + 2;
        var end = line.IndexOf('"', start);
        return line.AsSpan().Slice(start, end - start);
    }

    //TODO Parse Post and Answer separately
    private static PostModel ParsePostXmlRowLine(this string line)
    {
        var titleSpan = line.GetValue(nameof(PostModel.Title));
        var bodySpan = line.GetValue(nameof(PostModel.Body));
        var acceptedAnswerIdSpan = line.GetValue(nameof(PostModel.AcceptedAnswerId));

        return new PostModel
        {
            Id = int.Parse(line.GetValue(nameof(PostModel.Id))),
            Title = !titleSpan.IsEmpty ? titleSpan.ToString() : string.Empty,
            Body = !bodySpan.IsEmpty ? bodySpan.ToString() : string.Empty,
            PostTypeId = (PostType)int.Parse(line.GetValue(nameof(PostModel.PostTypeId))),
            LastActivityDate = DateTimeOffset.Parse(line.GetValue(nameof(PostModel.LastActivityDate))),
            AcceptedAnswerId = acceptedAnswerIdSpan.IsEmpty ? (int?)null : int.Parse(acceptedAnswerIdSpan)
        };
    }
}