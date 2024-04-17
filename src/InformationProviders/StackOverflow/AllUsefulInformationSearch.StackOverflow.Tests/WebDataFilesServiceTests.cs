using AllUsefulInformationSearch.DataAccess.Common;
using AllUsefulInformationSearch.StackOverflow.DataAccess;
using AllUsefulInformationSearch.StackOverflow.Workflows;

namespace AllUsefulInformationSearch.StackOverflow.Tests;

[TestClass]
public class WebDataFilesServiceTests
{
    public TestContext TestContext { get; set; }
    
    [TestMethod]
    [Ignore("Only for integration")]
    public async Task CanDownloadAndParseAndSaveFilesToDb()
    {
        //TODO Optimize this later
        var dbConnectionFactory = new DbConnectionFactory("Host=localhost;Port=5432;Database=AllUsefulInformationSearch_StackOverflow;Username=stackoverflowdbuser;Password=Qazwsx123!");
        var repository = new WebDataFilesRepository(dbConnectionFactory);
        var logger = new TestContextLogger<WebArchiveParser>(TestContext);
        var parser = new WebArchiveParser(logger);
        var service = new WebDataFilesService(repository, parser);
        await service.SynchronizeWebDataFilesAsync();
    }
}