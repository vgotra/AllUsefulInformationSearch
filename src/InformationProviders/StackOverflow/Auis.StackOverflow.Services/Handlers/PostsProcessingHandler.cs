using Auis.StackOverflow.DataAccess.Repositories;

namespace Auis.StackOverflow.Services.Handlers;

public class PostsProcessingHandler(IOptions<StackOverflowOptions> options, IServiceProvider serviceProvider, IMediator mediator, ILogger<PostsProcessingHandler> logger) : ICommandHandler<PostsProcessingCommand>
{
    public async ValueTask<Unit> Handle(PostsProcessingCommand command, CancellationToken cancellationToken)
    {
        var webFilePaths = command.WebDataFileEntity.ToWebFilePaths(options.Value);
        var webDataFilesRepository = serviceProvider.GetRequiredService<IWebDataFilesRepository>();

        try
        {
            logger.LogInformation("Processing started for {WebFileName}", command.WebDataFileEntity.Name);
            await webDataFilesRepository.SetProcessingStatusAsync(command.WebDataFileEntity, ProcessingStatus.InProgress, cancellationToken);
            var result = await mediator.Send(new ArchiveFileProcessingRequest(webFilePaths), cancellationToken);
            var response = await mediator.Send(new PostModificationRequest(result.Posts), cancellationToken);
            await mediator.Send(new PostsSynchronizationCommand(webFilePaths, response.Posts), cancellationToken);
            await webDataFilesRepository.SetProcessingStatusAsync(command.WebDataFileEntity, ProcessingStatus.Processed, cancellationToken);
            logger.LogInformation("Processing completed for {WebFileName}", command.WebDataFileEntity.Name);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error during processing of {WebFileName}", command.WebDataFileEntity.Name);
            await webDataFilesRepository.SetProcessingStatusAsync(command.WebDataFileEntity, ProcessingStatus.Failed, cancellationToken);
            throw;
        }

        return Unit.Value;
    }
}