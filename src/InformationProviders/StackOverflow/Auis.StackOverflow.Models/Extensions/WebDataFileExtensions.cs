﻿namespace Auis.StackOverflow.Models.Extensions;

public static class WebDataFileExtensions
{
    public static WebFilePaths ToWebFilePaths(this WebDataFileEntity webDataFile) =>
        new()
        {
            WebDataFileId = webDataFile.Id,
            WebDataFileSize = webDataFile.Size,
            WebFileUri = webDataFile.Link, 
            TemporaryDownloadPath = Path.GetTempFileName(), 
            ArchiveOutputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())
        };
}