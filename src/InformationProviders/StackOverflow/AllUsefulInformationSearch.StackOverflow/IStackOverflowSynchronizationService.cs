namespace AllUsefulInformationSearch.StackOverflow;

public interface IStackOverflowSynchronizationService
{
    Task SynchronizeAsync(CancellationToken cancellationToken = default);
}