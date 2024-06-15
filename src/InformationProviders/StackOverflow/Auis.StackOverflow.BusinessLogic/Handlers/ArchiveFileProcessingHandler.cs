using Auis.StackOverflow.BusinessLogic.Utilities;

namespace Auis.StackOverflow.BusinessLogic.Handlers;

public sealed class ArchiveFileProcessingHandler(IFileUtilityService fileUtilityService, IParsingService parsingService) : IRequestHandler<ArchiveFileProcessingRequest, ArchiveFileProcessingResponse>
{
    public async ValueTask<ArchiveFileProcessingResponse> Handle(ArchiveFileProcessingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await fileUtilityService.ExtractArchiveFileAsync(request.WebFileInformation, cancellationToken);
            fileUtilityService.DeleteTemporaryFiles(request.WebFileInformation);

            var posts = await parsingService.ParsePostsAsync(request.WebFileInformation, cancellationToken);
            return new ArchiveFileProcessingResponse(posts);
        }
        finally
        {
            fileUtilityService.DeleteExtractedFiles(request.WebFileInformation);
        }
    }
}

