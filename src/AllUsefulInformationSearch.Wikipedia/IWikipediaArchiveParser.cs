namespace AllUsefulInformationSearch.Wikipedia;

public interface IWikipediaArchiveParser
{
    Task<List<WikipediaDataFile>> GetFileInfoListAsync(CancellationToken cancellationToken = default);
}