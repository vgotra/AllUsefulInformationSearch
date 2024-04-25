using AllUsefulInformationSearch.StackOverflow.PostsParser.FileUtilityServices;

namespace AllUsefulInformationSearch.StackOverflow.Tests;

[TestClass]
public class WebDataFilesServiceTests : BaseTests
{
    [TestMethod]
    public async Task CanDownloadAndParseAndSaveFilesToDb()
    {
        //TODO Improve it later
        const string stackOverflowArchiveUrl = "https://archive.org/download/stackexchange";
        
        var dbContext = new StackOverflowDbContextTestFactory().CreateDbContext(null!);
        var repository = new WebDataFilesRepository(dbContext);
        var logger = new TestContextLogger<WebArchiveParser>(TestContext);
        var parser = new WebArchiveParser(logger);
        var service = new WebDataFilesService(repository, parser);
        await service.SynchronizeWebDataFilesAsync();
        var itemsCount = await dbContext.WebDataFiles.CountAsync();
        Assert.IsTrue(itemsCount > 0);
        
        var webDataFile = await dbContext.WebDataFiles.FirstOrDefaultAsync(x => x.Name .StartsWith("3dprinting.meta.stackexchange.com"));
        Assert.IsNotNull(webDataFile);

        var fileUtilityService = new WindowsFileUtilityService(); //TODO Check for OS 
        var file = $"{stackOverflowArchiveUrl}/{webDataFile.Link}";
        await new WebArchiveFileService(fileUtilityService).UnzipWebFileAsync(file);
    }
}