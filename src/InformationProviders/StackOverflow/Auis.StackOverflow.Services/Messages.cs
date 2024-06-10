namespace Auis.StackOverflow.Services;

public sealed record RefreshWebArchiveFilesRequest : IRequest<Unit>;
public sealed record WebArchiveParserQuery : IQuery<WebArchiveParserResponse>;
public sealed record WebArchiveParserResponse(List<StackOverflowDataFile> Files);
public sealed record WebArchiveFilesSaveCommand(List<StackOverflowDataFile> Files) : ICommand<Unit>;