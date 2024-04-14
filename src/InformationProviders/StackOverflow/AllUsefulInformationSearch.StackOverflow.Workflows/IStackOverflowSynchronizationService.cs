namespace AllUsefulInformationSearch.StackOverflow.Workflows;

public interface IStackOverflowSynchronizationService
{
    Task SynchronizeAsync(CancellationToken cancellationToken = default);
}