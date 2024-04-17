namespace AllUsefulInformationSearch.StackOverflow.Tests;

[TestClass]
public class UnitTests
{
    public TestContext TestContext { get; set; }
    
    [TestMethod]
    [Ignore("Only for integration")]
    public async Task CanDownloadAndParseLinksToFiles()
    {
        var logger = new TestContextLogger<WebArchiveParser>(TestContext);
        var parser = new WebArchiveParser(logger);
        var items = await parser.GetFileInfoListAsync();
        Assert.IsTrue(items.Count > 0);
    }
}