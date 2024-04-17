namespace AllUsefulInformationSearch.StackOverflow.WebParser;

public interface IWebArchiveParser
{
    Task<List<StackOverflowDataFile>> GetFileInfoListAsync(CancellationToken cancellationToken = default);
}