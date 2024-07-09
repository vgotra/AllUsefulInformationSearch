namespace Auis.Wikipedia.BusinessLogic.Services;

public interface IWebArchivesSynchronizationService
{
    ValueTask SynchronizeWebArchiveFiles(CancellationToken cancellationToken = default);
}