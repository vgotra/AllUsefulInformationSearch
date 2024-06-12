namespace Auis.StackOverflow.Services.Handlers;

public sealed record RefreshWebArchiveFilesRequest : IRequest<Unit>;
public sealed record WebArchiveParserQuery : IQuery<WebArchiveParserResponse>;
public sealed record WebArchiveParserResponse(List<WebDataFile> Files);
public sealed record WebArchiveFilesSaveCommand(List<WebDataFile> Files) : ICommand<Unit>;
public sealed record PostsSynchronizationCommand(WebFileInformation WebFileInformation, List<PostModel> ModifiedPosts) : ICommand<Unit>;
public sealed record PostModificationRequest(List<PostModel> Posts) : IRequest<PostModificationResponse>;
public sealed record PostModificationResponse(List<PostModel> Posts);
public sealed record ArchiveFileProcessingRequest(WebFileInformation WebFileInformation) : IRequest<ArchiveFileProcessingResponse>;
public sealed record ArchiveFileProcessingResponse(List<PostModel> Posts);
public sealed record PostsProcessingCommand(WebDataFileEntity WebDataFileEntity) : ICommand<Unit>;