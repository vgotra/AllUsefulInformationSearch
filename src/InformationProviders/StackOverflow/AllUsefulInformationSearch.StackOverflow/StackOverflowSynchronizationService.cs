using AllUsefulInformationSearch.Common;

namespace AllUsefulInformationSearch.StackOverflow;

public class StackOverflowSynchronizationService(IStackOverflowArchiveParser parser) : IStackOverflowSynchronizationService
{
    public async Task SynchronizeAsync(CancellationToken cancellationToken = default)
    {
        var dataFiles = new List<InformationSourceDataFile>();
        var dataFileDict = dataFiles.ToDictionary(x => x.Name, x => x);
        var archiveFiles = await parser.GetFileInfoListAsync(cancellationToken);
        var archiveFileDict = archiveFiles.ToDictionary(x => x.Name, x => x);
        var newFiles = archiveFileDict.Values.Where(x => !dataFileDict.ContainsKey(x.Name)).ToList();
        var updatedFiles = dataFileDict.Values.Where(x => archiveFileDict.ContainsKey(x.Name) && x.ProviderLastModified != archiveFileDict[x.Name].LastModified).ToList();
        // var deletedFiles = dataFileDict.Values.Where(x => !archiveFileDict.ContainsKey(x.Name)).ToList();
        //TODO Update db with new and updated files
        //TODO Add logic for synchronization and sending message to service which will download and extract files and process data
    }
}