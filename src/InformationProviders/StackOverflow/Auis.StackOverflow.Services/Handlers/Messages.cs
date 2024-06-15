namespace Auis.StackOverflow.Services.Handlers;

public sealed record RefreshWebArchiveFilesRequest : IRequest<Unit>;
public sealed record WebArchiveParserQuery : IQuery<WebArchiveParserResponse>;
public sealed record WebArchiveParserResponse(List<WebDataFile> Files);
public sealed record WebArchiveFilesSaveCommand(List<WebDataFile> Files) : ICommand<Unit>;
public sealed record PostsSynchronizationCommand(WebFileInformation WebFileInformation, List<PostEntity> ModifiedPosts) : ICommand<Unit>;
public sealed record PostModificationRequest(List<PostEntity> Posts) : IRequest<PostModificationResponse>;
public sealed record PostModificationResponse(List<PostEntity> Posts);
public sealed record ArchiveFileProcessingRequest(WebFileInformation WebFileInformation) : IRequest<ArchiveFileProcessingResponse>;
public sealed record ArchiveFileProcessingResponse(List<PostEntity> Posts);
public sealed record PostsProcessingCommand(WebDataFileEntity WebDataFileEntity) : ICommand<Unit>;