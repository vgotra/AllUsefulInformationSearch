namespace AllUsefulInformationSearch.StackOverflow.Workflows;

public interface IStackOverflowProcessingSubWorkflow
{
    Task ExecuteAsync(WebDataFileEntity webDataFileEntity, CancellationToken cancellationToken = default);
}