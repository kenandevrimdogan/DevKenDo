using DevKenDo.Identity.Application.Pipelines.Infrastructure;
using Microsoft.AspNetCore.Http;
using MediatR;

namespace DevKenDo.Identity.Application.Pipelines;

public class CorrelationIdBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationIdBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var correlationId = _httpContextAccessor.HttpContext?.Items["CorrelationId"]?.ToString()
                            ?? Guid.NewGuid().ToString();

        if (request is ICorrelatable correlatable)
        {
            correlatable.CorrelationId = correlationId;
        }

        return await next();
    }
}