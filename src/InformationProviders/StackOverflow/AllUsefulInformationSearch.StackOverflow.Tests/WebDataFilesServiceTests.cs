namespace AllUsefulInformationSearch.StackOverflow.Tests;

[TestClass]
public class WebDataFilesServiceTests : BaseTests
{
    [TestMethod]
    public async Task CanDownloadAndParseAndSaveFilesToDb()
    {
        var dbContext = new StackOverflowDbContextTestFactory().CreateDbContext(null!);
        var repository = new WebDataFilesRepository(dbContext);
        var logger = new TestContextLogger<WebArchiveParser>(TestContext);
        var parser = new WebArchiveParser(logger);
        var service = new WebDataFilesService(repository, parser);
        await service.SynchronizeWebDataFilesAsync();
        var itemsCount = await dbContext.WebDataFiles.CountAsync();
        Assert.IsTrue(itemsCount > 0);
    }
}