namespace Auis.StackOverflow.Workflows;

public interface IStackOverflowProcessingWorkflow
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}