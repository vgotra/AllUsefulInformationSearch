namespace Auis.StackOverflow.Services.Handlers;

public sealed class ArchiveFileProcessingHandler(IFileUtilityService fileUtilityService) : IRequestHandler<ArchiveFileProcessingRequest, ArchiveFileProcessingResponse>
{
    public async ValueTask<ArchiveFileProcessingResponse> Handle(ArchiveFileProcessingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await fileUtilityService.ExtractArchiveFileAsync(request.WebFileInformation, cancellationToken);
            fileUtilityService.DeleteTemporaryFiles(request.WebFileInformation);

            //TODO To Deserialization service
            var posts = await Path.Combine(request.WebFileInformation.ArchiveOutputDirectory, "Posts.xml").DeserializePostsAsync(request.WebFileInformation.WebDataFileSize, cancellationToken);
            posts.ForEach(p =>
            {
                p.WebDataFileId = request.WebFileInformation.WebDataFileId;
                p.AcceptedAnswer = posts.FirstOrDefault(x => x.Id == p.AcceptedAnswerId && x.PostTypeId == PostType.Answer); // improve this
            });

            // get only useful answered posts
            posts = posts.Where(x => Defaults.UsefulPostTypes.Contains(x.PostTypeId) && x.AcceptedAnswer != null).ToList();
            if (posts == null || posts.Count == 0)
                throw new InvalidDataException();

            return new ArchiveFileProcessingResponse(posts);
        }
        finally
        {
            fileUtilityService.DeleteExtractedFiles(request.WebFileInformation);
        }
    }
}

