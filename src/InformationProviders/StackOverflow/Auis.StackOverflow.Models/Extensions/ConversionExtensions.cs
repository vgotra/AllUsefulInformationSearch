﻿namespace Auis.StackOverflow.Models.Extensions;

public static class ConversionExtensions
{
    public static WebDataFileEntity ToEntity(this StackOverflowDataFile file) =>
        new()
        {
            Name = file.Name,
            Link = file.Link,
            Size = file.Size,
            ExternalLastModified = file.LastModified,
            ProcessingStatus = ProcessingStatus.New
        };
}