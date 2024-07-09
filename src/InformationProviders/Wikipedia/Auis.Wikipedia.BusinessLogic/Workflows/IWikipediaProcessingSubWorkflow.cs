namespace Auis.Wikipedia.BusinessLogic.Workflows;

public interface IWikipediaProcessingSubWorkflow
{
    Task ExecuteAsync(WebDataFileEntity webDataFileEntity, CancellationToken cancellationToken = default);
}