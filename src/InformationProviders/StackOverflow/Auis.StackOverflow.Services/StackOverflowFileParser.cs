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

    private static string? GetValue(this string line, string attributeName)
    {
        var start = line.IndexOf(attributeName + "=\"", StringComparison.OrdinalIgnoreCase);
        if (start == -1)
            return default;
        start += attributeName.Length + 2;
        var end = line.IndexOf('"', start);
        return line[start..end];
    }

    //TODO Parse Post and Answer separately
    private static PostModel ParsePostXmlRowLine(this string line)
    {
        var id = int.Parse(line.GetValue(nameof(PostModel.Id))!);
        var postTypeId = (PostType)int.Parse(line.GetValue(nameof(PostModel.PostTypeId))!);
        var body = line.GetValue(nameof(PostModel.Body)) ?? string.Empty;
        var lastActivityDate = DateTimeOffset.Parse(line.GetValue(nameof(PostModel.LastActivityDate))!);
        var title = line.GetValue(nameof(PostModel.Body)) ?? string.Empty;

        var acceptedAnswerIdVal = line.GetValue(nameof(PostModel.AcceptedAnswerId));
        var acceptedAnswerId = acceptedAnswerIdVal == null ? (int?)null : int.Parse(acceptedAnswerIdVal);

        return new PostModel
        {
            Id = id,
            Title = title,
            Body = body,
            PostTypeId = postTypeId,
            LastActivityDate = lastActivityDate,
            AcceptedAnswerId = acceptedAnswerId
        };
    }
}