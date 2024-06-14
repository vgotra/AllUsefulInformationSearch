namespace Auis.StackOverflow.Services.Workflows;

public interface IStackOverflowProcessingSubWorkflow
{
    Task ExecuteAsync(WebDataFileEntity webDataFileEntity, CancellationToken cancellationToken = default);
}