﻿namespace Auis.StackOverflow.Models.Common;

public record WebFilePaths
{
    public int WebDataFileId { get; set; }
    public long WebDataFileSize { get; set; }
    public string WebFileUri { get; set; } = string.Empty;
    public string TemporaryDownloadPath { get; set; } = string.Empty;
    public string ArchiveOutputDirectory { get; set; } = string.Empty;
}