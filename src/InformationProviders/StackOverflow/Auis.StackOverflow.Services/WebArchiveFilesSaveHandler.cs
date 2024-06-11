using Microsoft.Extensions.DependencyInjection;

namespace Auis.StackOverflow.Services;

public class WebArchiveFilesSaveHandler(IServiceProvider serviceProvider) : ICommandHandler<WebArchiveFilesSaveCommand>
{
    public async ValueTask<Unit> Handle(WebArchiveFilesSaveCommand command, CancellationToken cancellationToken)
    {
        var dbContext = serviceProvider.GetRequiredService<StackOverflowDbContext>();
        var archiveFileDict = command.Files.ToDictionary(x => x.Name, x => x);
        var dataFiles = await dbContext.WebDataFiles.ToListAsync(cancellationToken);
        var dataFileDict = dataFiles.ToDictionary(x => x.Name, x => x);

        var newFiles = archiveFileDict.Values.Where(x => !dataFileDict.ContainsKey(x.Name));
        var updatedFiles = dataFileDict.Values.Where(x => archiveFileDict.ContainsKey(x.Name) && x.ExternalLastModified != archiveFileDict[x.Name].LastModified).ToList();
        // No need to delete some data files, because they can be useful

        updatedFiles.ForEach(x => x.ProcessingStatus = ProcessingStatus.Updated);
        var webFiles = newFiles.Select(x => x.ToEntity()).Concat(updatedFiles).ToList();

        Defaults.AllFileNamesToSkip.ForEach(x =>
        {
            var webFile = webFiles.FirstOrDefault(y => x.Equals(y.Name, StringComparison.InvariantCultureIgnoreCase)); //TODO Add Apply/With/etc extensions
            if (webFile != null)
                webFile.IsSynchronizationEnabled = false;
        });

        if (webFiles.Count > 0)
        {
            await dbContext.AddRangeAsync(webFiles, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}