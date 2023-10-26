using AllUsefulInformationSearch.Common.Http.Extensions;

namespace AllUsefulInformationSearch.Common.Http;

public class HttpClientWrapper : HttpClient
{
    private readonly HttpClient _httpClient;

    public HttpClientWrapper() => _httpClient = new HttpClient();
    
    public HttpClientWrapper(HttpClient httpClient) => _httpClient = httpClient;

    public virtual Task<string> GetStringAsync(string? requestUri, CancellationToken token = default) => _httpClient.GetAsync(requestUri, token).GetResponseAsStringAsync();
}