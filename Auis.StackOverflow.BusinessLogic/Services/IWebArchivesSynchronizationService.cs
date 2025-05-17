namespace Auis.StackOverflow.BusinessLogic.Services;

public interface IWebArchivesSynchronizationService
{
    ValueTask SynchronizeWebArchiveFiles(CancellationToken cancellationToken = default);
}