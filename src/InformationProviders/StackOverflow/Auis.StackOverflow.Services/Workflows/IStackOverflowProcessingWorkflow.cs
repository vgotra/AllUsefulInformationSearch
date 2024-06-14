namespace Auis.StackOverflow.Services.Workflows;

public interface IStackOverflowProcessingWorkflow
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}