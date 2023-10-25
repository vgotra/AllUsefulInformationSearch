using AllUsefulInformationSearch.Common.Http;

namespace AllUsefulInformationSearch.StackOverflow.Tests;

public class UnitTests
{
    [Test]
    public async Task CanDownloadAndParseLinksToFiles()
    {
        var httpClientMock = new Mock<HttpClientWrapper>();
        httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns("CorrectStackOverflowResponse.html".GetTestFileContentAsync());

        var httpClientFactoryMock = new Mock<IHttpClientFactoryWrapper>();
        httpClientFactoryMock.Setup(x => x.CreateClient()).Returns(httpClientMock.Object);

        var parser = new StackOverflowArchiveParser(httpClientFactoryMock.Object);
        var items = await parser.GetDataFileInfoListAsync();

        Assert.That(items.Count, Is.GreaterThan(0));
    }
}