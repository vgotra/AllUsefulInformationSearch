using AllUsefulInformationSearch.StackOverflow.Models.Common;

namespace AllUsefulInformationSearch.StackOverflow.Services;

public interface IWebArchiveParserService
{
    Task<List<StackOverflowDataFile>> GetFileInfoListAsync(CancellationToken cancellationToken = default);
}