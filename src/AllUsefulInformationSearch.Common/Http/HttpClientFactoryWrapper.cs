namespace AllUsefulInformationSearch.Common.Http;

public class HttpClientFactoryWrapper : IHttpClientFactoryWrapper
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpClientFactoryWrapper(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public HttpClientWrapper CreateClient() => new(_httpClientFactory.CreateClient());

    public HttpClientWrapper CreateClient(string name) => new(_httpClientFactory.CreateClient(name));
}