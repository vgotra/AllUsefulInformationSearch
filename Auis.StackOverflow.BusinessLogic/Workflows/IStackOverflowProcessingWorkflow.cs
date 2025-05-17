namespace Auis.StackOverflow.BusinessLogic.Workflows;

public interface IStackOverflowProcessingWorkflow
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}