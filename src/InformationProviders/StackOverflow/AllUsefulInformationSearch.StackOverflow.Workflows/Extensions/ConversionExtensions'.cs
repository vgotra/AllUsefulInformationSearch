using AllUsefulInformationSearch.StackOverflow.Common;
using AllUsefulInformationSearch.StackOverflow.DataAccess.Entities;

namespace AllUsefulInformationSearch.StackOverflow.Workflows.Extensions;

public static class ConversionExtensions
{
    public static WebDataFileEntity ToEntity(this StackOverflowDataFile file) =>
        new()
        {
            Id = Guid.NewGuid(),
            Name = file.Name,
            Link = file.Link,
            Size = file.Size,
            LastModified = file.LastModified,
            ProcessingStatus = ProcessingStatus.New,
            LastUpdated = DateTime.UtcNow
        };
}