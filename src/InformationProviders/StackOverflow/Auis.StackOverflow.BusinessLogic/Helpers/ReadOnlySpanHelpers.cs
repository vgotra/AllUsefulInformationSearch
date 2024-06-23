namespace Auis.StackOverflow.BusinessLogic.Helpers;

public static class ReadOnlySpanHelpers
{
    public static ReadOnlySpan<char> GetValue(this string line, string attributeName)
    {
        var start = line.IndexOf(attributeName + "=\"", StringComparison.OrdinalIgnoreCase);
        if (start == -1)
            return default;
        start += attributeName.Length + 2;
        var end = line.IndexOf('"', start);
        return line.AsSpan().Slice(start, end - start);
    }
}