namespace AllUsefulInformationSearch.Tests.Common;

public static class TestFileExtensions
{
    public static string GetTestFilePath(this string fileName, string folder = "TestFiles") =>
        Path.Combine(folder, fileName);

    public static string GetTestFileContent(this string fileName, string folder = "TestFiles") =>
        File.ReadAllText(GetTestFilePath(fileName, folder));

    public static async Task<string> GetTestFileContentAsync(this string fileName, string folder = "TestFiles") =>
        await File.ReadAllTextAsync(GetTestFilePath(fileName, folder));
}