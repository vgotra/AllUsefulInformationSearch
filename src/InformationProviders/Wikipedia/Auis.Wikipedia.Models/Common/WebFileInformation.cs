namespace Auis.Wikipedia.Models.Common;

public sealed record WebFileInformation
{
    public int WebDataFileId { get; set; }
    public long FileSize { get; set; }
    public string FileUri { get; set; } = string.Empty;
    public string TemporaryDownloadPath { get; set; } = string.Empty;
    public string ArchiveOutputDirectory { get; set; } = string.Empty;
    public FileLocation FileLocation { get; set; } = FileLocation.Web;
}