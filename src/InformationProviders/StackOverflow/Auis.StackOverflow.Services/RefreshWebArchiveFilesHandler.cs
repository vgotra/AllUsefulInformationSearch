namespace Auis.StackOverflow.Services;

public class RefreshWebArchiveFilesHandler(IMediator mediator) : IRequestHandler<RefreshWebArchiveFilesRequest>
{
    public async ValueTask<Unit> Handle(RefreshWebArchiveFilesRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new WebArchiveParserQuery(), cancellationToken);
        var result = await mediator.Send(new WebArchiveFilesSaveCommand(response.Files), cancellationToken);
        return result;
    }
}