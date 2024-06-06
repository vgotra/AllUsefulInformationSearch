namespace Auis.StackOverflow.Services;

public interface IWebArchiveParserService
{
    Task<List<StackOverflowDataFile>> GetFileInfoListAsync(CancellationToken cancellationToken = default);
}