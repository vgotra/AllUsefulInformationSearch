namespace Auis.Wikipedia.BusinessLogic.Workflows;

public interface IWikipediaProcessingWorkflow
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}