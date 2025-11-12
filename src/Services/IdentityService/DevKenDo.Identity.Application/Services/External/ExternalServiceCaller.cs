using Microsoft.AspNetCore.Http;

namespace DevKenDo.Identity.Application.Services.External;

public class ExternalServiceCaller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExternalServiceCaller(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> GetDataAsync(string url)
    {
        var client = _httpClientFactory.CreateClient("ExternalServiceClient");

        var correlationId = _httpContextAccessor.HttpContext?.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-Correlation-ID", correlationId);

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}