namespace AllUsefulInformationSearch.StackOverflow.Models.Common;

public record WebFilePaths
{
    public string WebFileUri { get; set; } = string.Empty;
    public string TemporaryDownloadPath { get; set; } = string.Empty;
    public string ArchiveOutputDirectory { get; set; } = string.Empty;
}