namespace Auis.StackOverflow.Tests;

[TestClass]
public class UnitTests : BaseTests
{
    [TestMethod]
#if !DEBUG
    [Ignore("This test is for manual execution only")]
#endif
    public async Task CanDownloadAndParseLinksToFiles()
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("https://archive.org/download/stackexchange/") };
        var logger = new TestContextLogger<WebArchiveParserService>(TestContext);
        var parser = new WebArchiveParserService(httpClient, logger);
        var items = await parser.GetFileInfoListAsync();
        Assert.IsTrue(items.Count > 0);
    }
}