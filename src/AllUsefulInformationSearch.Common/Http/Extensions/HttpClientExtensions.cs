namespace AllUsefulInformationSearch.Common.Http.Extensions;

public static class HttpClientExtensions
{
    public static async Task<string> GetResponseAsStringAsync(this Task<HttpResponseMessage> response) 
    {
        var responseMessage = await response;
        return responseMessage is { IsSuccessStatusCode: true, Content: not null }
            ? await responseMessage.Content.ReadAsStringAsync() ?? string.Empty
            : string.Empty;
    }
}