namespace Auis.StackOverflow.BusinessLogic.Workflows;

public interface IStackOverflowProcessingSubWorkflow
{
    Task ExecuteAsync(WebDataFileEntity webDataFileEntity, CancellationToken cancellationToken = default);
}