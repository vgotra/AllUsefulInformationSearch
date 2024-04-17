namespace AllUsefulInformationSearch.StackOverflow.Workflows;

public class WebDataFilesService : IWebDataFilesService
{
    private readonly IWebDataFilesRepository _webDataFilesRepository;
    private readonly IWebArchiveParser _parser;

    public WebDataFilesService(IWebDataFilesRepository webDataFilesRepository, IWebArchiveParser parser)
    {
        _webDataFilesRepository = webDataFilesRepository;
        _parser = parser;
    }

    public async Task SynchronizeWebDataFilesAsync(CancellationToken cancellationToken = default)
    {
        var archiveFiles = await _parser.GetFileInfoListAsync(cancellationToken);
        var archiveFileDict = archiveFiles.ToDictionary(x => x.Name, x => x);
        var dataFiles = await _webDataFilesRepository.GetWebDataFileListAsync(cancellationToken);
        var dataFileDict = dataFiles.ToDictionary(x => x.Name, x => x);

        var newFiles = archiveFileDict.Values.Where(x => !dataFileDict.ContainsKey(x.Name)).ToList();
        var updatedFiles = dataFileDict.Values.Where(x => archiveFileDict.ContainsKey(x.Name) && x.LastModified != archiveFileDict[x.Name].LastModified).ToList();
        // No need to delete some data files, because they can be useful

        if (newFiles.Count > 0)
        {
            var newWebFiles = newFiles.Select(x => x.ToEntity()).ToList();
            await _webDataFilesRepository.AddWebDataFileListAsync(newWebFiles, cancellationToken);
        }

        if (updatedFiles.Count > 0)
        {
            await _webDataFilesRepository.UpdateWebDataFileListAsync(updatedFiles, cancellationToken);
        }
    }
}