namespace AllUsefulInformationSearch.StackOverflow.Services;

public interface IWebDataFilesService
{
    Task SynchronizeWebDataFilesAsync(CancellationToken cancellationToken = default);
}