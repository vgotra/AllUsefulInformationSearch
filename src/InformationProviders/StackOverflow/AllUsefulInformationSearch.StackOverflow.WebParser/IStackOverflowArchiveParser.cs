namespace AllUsefulInformationSearch.StackOverflow.WebParser;

public interface IStackOverflowArchiveParser
{
    Task<List<StackOverflowDataFile>> GetFileInfoListAsync(CancellationToken cancellationToken = default);
}