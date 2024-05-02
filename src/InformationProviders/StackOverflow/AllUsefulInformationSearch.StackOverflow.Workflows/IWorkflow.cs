namespace AllUsefulInformationSearch.StackOverflow.Workflows;

public interface IWorkflow
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}