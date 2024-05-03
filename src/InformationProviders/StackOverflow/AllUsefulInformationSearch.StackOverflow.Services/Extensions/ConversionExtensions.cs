using AllUsefulInformationSearch.StackOverflow.Models.Common;

namespace AllUsefulInformationSearch.StackOverflow.Services.Extensions;

public static class ConversionExtensions
{
    public static WebDataFileEntity ToEntity(this StackOverflowDataFile file) =>
        new()
        {
            Id = Guid.NewGuid(),
            Name = file.Name,
            Link = file.Link,
            Size = file.Size,
            ExternalLastModified = file.LastModified,
            ProcessingStatus = ProcessingStatus.New,
            LastUpdated = DateTime.UtcNow
        };
}