namespace Auis.Wikipedia.BusinessLogic.Services;

public interface IWebArchiveParserService
{
    ValueTask<List<WebDataFile>> GetWebDataFilesAsync(CancellationToken cancellationToken = default);
}