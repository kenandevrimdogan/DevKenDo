using DevKenDo.Identity.API.Middleware.Model;

namespace DevKenDo.Identity.API.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(CorrelationIdConstants.Header, out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        context.Items[CorrelationIdConstants.ContextKey] = correlationId;
        context.Response.Headers[CorrelationIdConstants.Header] = correlationId;

        await _next(context);
    }
}