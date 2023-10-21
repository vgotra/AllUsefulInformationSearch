namespace AllDataSearch.StackOverflow.Tests;

public class IntegrationTests
{
    [Test, Ignore("To not download the whole archive every time during CI/CD")]
    public async Task CanDownloadAndParseLinksToFiles()
    {
        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

        var parser = new StackOverflowArchiveParser(httpClientFactory.Object);
        var items = await parser.GetDataFileInfoListAsync();
        
        Assert.That(items.Count, Is.GreaterThan(0));
    }
}