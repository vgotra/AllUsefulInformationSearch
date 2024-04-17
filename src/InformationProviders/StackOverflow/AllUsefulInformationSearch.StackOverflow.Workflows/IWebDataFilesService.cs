namespace AllUsefulInformationSearch.StackOverflow.Workflows;

public interface IWebDataFilesService
{
    Task SynchronizeWebDataFilesAsync(CancellationToken cancellationToken = default);
}