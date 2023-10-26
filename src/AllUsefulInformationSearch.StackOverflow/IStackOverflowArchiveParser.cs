namespace AllUsefulInformationSearch.StackOverflow;

public interface IStackOverflowArchiveParser
{
    Task<List<StackOverflowDataFile>> GetFileInfoListAsync(CancellationToken cancellationToken = default);
}