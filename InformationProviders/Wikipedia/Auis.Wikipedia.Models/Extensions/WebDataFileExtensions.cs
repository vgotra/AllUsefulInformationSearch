using Auis.Wikipedia.Common.Options;

namespace Auis.Wikipedia.Models.Extensions;

public static class WebDataFileExtensions
{
    public static WebFileInformation ToWebFileInformation(this WebDataFileEntity webDataFile, WikipediaOptions options) =>
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