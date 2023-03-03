namespace App.Middleware;

public class ProxyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpClient _httpClient;

    public ProxyMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory)
    {
        _next = next;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task Invoke(HttpContext context)
    {
        var externalUri = new Uri($"http://localhost:5174/{context.Request.Path}");
        var request = new HttpRequestMessage
        {
            Method = new HttpMethod(context.Request.Method),
            RequestUri = externalUri
        };

        // Copy headers from original request to external request
        foreach (var header in context.Request.Headers)
        {
            request.Headers.Add(header.Key, header.Value.ToArray());
        }

        // Make the request to the external program
        var response = await _httpClient.SendAsync(request);

        // Copy headers from external response to original response
        foreach (var header in response.Headers)
        {
            context.Response.Headers.Add(header.Key, header.Value.ToArray());
        }

        // Return the external response to the original caller
        context.Response.StatusCode = (int) response.StatusCode;
        context.Response.ContentType = response.Content.Headers.ContentType?.MediaType;
        await response.Content.CopyToAsync(context.Response.Body);
    }
}
