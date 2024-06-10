namespace Auis.StackOverflow.Services.Handlers;

public sealed record WebArchiveParserResponse
{
    public List<StackOverflowDataFile> DataFiles { get; set; } = new();
}