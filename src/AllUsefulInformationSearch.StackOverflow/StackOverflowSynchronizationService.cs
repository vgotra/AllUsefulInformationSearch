namespace AllUsefulInformationSearch.StackOverflow;

public class StackOverflowSynchronizationService : IStackOverflowSynchronizationService
{
    private readonly IInformationSourceDataFileRepository _repository;
    private readonly IStackOverflowArchiveParser _parser;

    public StackOverflowSynchronizationService(IInformationSourceDataFileRepository repository, IStackOverflowArchiveParser parser)
    {
        _repository = repository;
        _parser = parser;
    }
    
    public async Task SynchronizeAsync(CancellationToken cancellationToken = default)
    {
        var dataFiles = await _repository.GetAllByProviderAsync(InformationProvider.StackOverflow, cancellationToken);
        var dataFileDict = dataFiles.ToDictionary(x => x.Name, x => x);
        var archiveFiles = await _parser.GetFileInfoListAsync(cancellationToken);
        var archiveFileDict = archiveFiles.ToDictionary(x => x.Name, x => x);
        var newFiles = archiveFileDict.Values.Where(x => !dataFileDict.ContainsKey(x.Name)).ToList();
        var updatedFiles = dataFileDict.Values.Where(x => archiveFileDict.ContainsKey(x.Name) && x.ProviderLastModified != archiveFileDict[x.Name].LastModified).ToList();
        // var deletedFiles = dataFileDict.Values.Where(x => !archiveFileDict.ContainsKey(x.Name)).ToList();
        //TODO Update db with new and updated files
        //TODO Add logic for synchronization and sending message to service which will download and extract files and process data
    }
}