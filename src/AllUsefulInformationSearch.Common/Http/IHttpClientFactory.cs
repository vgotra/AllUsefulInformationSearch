namespace AllUsefulInformationSearch.Common.Http;

public interface IHttpClientFactoryWrapper
{
    HttpClientWrapper CreateClient();
    HttpClientWrapper CreateClient(string name);
}