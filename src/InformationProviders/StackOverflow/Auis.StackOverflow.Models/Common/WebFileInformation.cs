﻿namespace Auis.StackOverflow.Models.Common;

public record WebFileInformation
{
    public int WebDataFileId { get; set; }
    public long WebDataFileSize { get; set; }
    public string FileUri { get; set; } = string.Empty;
    public string TemporaryDownloadPath { get; set; } = string.Empty;
    public string ArchiveOutputDirectory { get; set; } = string.Empty;
    public FileLocation FileLocation { get; set; } = FileLocation.Web;
}