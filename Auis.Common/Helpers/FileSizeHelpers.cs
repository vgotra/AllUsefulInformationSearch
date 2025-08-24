using System.Globalization;

namespace Auis.Common.Helpers;

public static class FileSizeHelpers
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
        fileSize.ToUpper().LastOrDefault() switch
        {
            'K' => FileSize.Kb,
            'M' => FileSize.Mb,
            'G' => FileSize.Gb,
            _ => 1
        };
}