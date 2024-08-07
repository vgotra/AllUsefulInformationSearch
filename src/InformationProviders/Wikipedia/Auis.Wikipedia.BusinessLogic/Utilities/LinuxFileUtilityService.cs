﻿using Auis.Wikipedia.BusinessLogic.Helpers;

namespace Auis.Wikipedia.BusinessLogic.Utilities;

public class LinuxFileUtilityService : FileUtilityServiceBase, IFileUtilityService
{
    public async Task ExtractArchiveFileAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default)
    {
        await DownloadFileAsync(webFileInformation, cancellationToken);
        var command = $"7za x {webFileInformation.TemporaryDownloadPath} -o{webFileInformation.ArchiveOutputDirectory}";
        await "/bin/bash".ExecuteProcessAsync($"-c \"{command}\"", cancellationToken);
    }
}