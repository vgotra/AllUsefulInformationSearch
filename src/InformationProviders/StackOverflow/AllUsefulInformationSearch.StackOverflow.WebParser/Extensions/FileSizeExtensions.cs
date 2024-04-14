using System.Globalization;

namespace AllUsefulInformationSearch.StackOverflow.WebParser.Extensions;

public static class FileSizeExtensions
{
    public static long GetFileSize(this string fileSize)
    {
        if (string.IsNullOrWhiteSpace(fileSize))
            return 0;

        var fileSizeWithoutMultiplier = float.Parse(fileSize[..^1].Replace(",",string.Empty), CultureInfo.InvariantCulture);
        var multiplier = GetMultiplier(fileSize);
        return (long)(fileSizeWithoutMultiplier * multiplier);
    }

    private static long GetMultiplier(string fileSize) =>
        fileSize.LastOrDefault() switch
        {
            'K' => 1024,
            'M' => 1024 * 1024,
            'G' => 1024 * 1024 * 1024,
            _ => 1
        };
}