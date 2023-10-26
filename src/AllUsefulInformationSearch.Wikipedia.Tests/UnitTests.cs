using AllUsefulInformationSearch.Common.Http;

namespace AllUsefulInformationSearch.Wikipedia.Tests;

public class UnitTests
{
    [Test]
    public async Task CanDownloadAndParseLinksToFiles()
    {
        var httpClientMock = new Mock<HttpClientWrapper>();
        httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns("CorrectWikipediaResponse.html".GetTestFileContentAsync());

        var httpClientFactoryMock = new Mock<IHttpClientFactoryWrapper>();
        httpClientFactoryMock.Setup(x => x.CreateClient()).Returns(httpClientMock.Object);

        var parser = new WikipediaArchiveParser(httpClientFactoryMock.Object);
        var items = await parser.GetFileInfoListAsync();

        Assert.That(items.Count, Is.GreaterThan(0));
    }
}