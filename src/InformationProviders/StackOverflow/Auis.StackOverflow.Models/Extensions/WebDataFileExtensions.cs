﻿using Auis.StackOverflow.Common.Options;

namespace Auis.StackOverflow.Models.Extensions;

public static class WebDataFileExtensions
{
    public static WebFileInformation ToWebFileInformation(this WebDataFileEntity webDataFile, StackOverflowOptions options) =>
        new()
        {
            WebDataFileId = webDataFile.Id,
            FileSize = webDataFile.Size,
            FileUri = options.IsNetworkShare ? Path.Combine(options.NetworkShareBasePath!, webDataFile.Name) : webDataFile.Link,
            TemporaryDownloadPath = Path.GetTempFileName(),
            ArchiveOutputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()),
            FileLocation = options.IsNetworkShare ? FileLocation.Network : FileLocation.Web
        };
}