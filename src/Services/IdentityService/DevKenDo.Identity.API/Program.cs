using DevKenDo.Identity.API.Endpoints;
using DevKenDo.Identity.API.Middleware;
using DevKenDo.Identity.Application.Commands.CreateUser;
using DevKenDo.Identity.Application.Pipelines;
using DevKenDo.Identity.Application.Services.External;
using DevKenDo.Identity.Infrastructure.Db;
using DevKenDo.Identity.Infrastructure.Events.Impl;
using DevKenDo.Identity.Infrastructure.Events.Infrastructure;
using DevKenDo.Identity.Infrastructure.Tenant.Impl;
using DevKenDo.Identity.Infrastructure.Tenant.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// Logging (Serilog)
// ---------------------------
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(ctx.Configuration);
});

// ---------------------------
// DbContext & Multi-Tenant
// ---------------------------
builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseInMemoryDatabase("IdentityDb"); // Memory DB
});

// Tenant Provider
builder.Services.AddScoped<ITenantProvider, TenantProvider>();

// ---------------------------
// MediatR & Pipelines
// ---------------------------
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CorrelationIdBehavior<,>));

// ---------------------------
// Polly / Circuit Breaker for ExternalServiceClient
// ---------------------------
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
    .WaitAndRetryAsync(
        3,
        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
        onRetry: (response, timespan, retryCount, context) =>
        {
            var correlationId = context.ContainsKey("CorrelationId") ? context["CorrelationId"] : "N/A";
            Log.Warning("Retry {RetryCount} for {CorrelationId} after {Delay}s due to {StatusCode}",
                retryCount, correlationId, timespan.TotalSeconds, response.Result?.StatusCode);
        });

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(
        5,
        TimeSpan.FromSeconds(30),
        onBreak: (result, breakDelay, context) =>
        {
            var correlationId = context.ContainsKey("CorrelationId") ? context["CorrelationId"] : "N/A";
            Log.Warning("Circuit opened for {CorrelationId} for {Delay}s due to {StatusCode}",
                correlationId, breakDelay.TotalSeconds, result.Result?.StatusCode);
        },
        onReset: context =>
        {
            var correlationId = context.ContainsKey("CorrelationId") ? context["CorrelationId"] : "N/A";
            Log.Information("Circuit reset for {CorrelationId}", correlationId);
        });

builder.Services.AddHttpClient("ExternalServiceClient")
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);

// External Service Caller
builder.Services.AddScoped<ExternalServiceCaller>();

// ---------------------------
// Outbox / Event Dispatcher
// ---------------------------
builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();
builder.Services.AddScoped<IOutboxService, OutboxService>();

// ---------------------------
// Swagger / API Explorer
// ---------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------------------
// Build App
// ---------------------------
var app = builder.Build();

// ---------------------------
// Middleware
// ---------------------------
app.UseMiddleware<CorrelationIdMiddleware>();

// ---------------------------
// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ---------------------------
// Endpoint’leri register et
// ---------------------------
app.MapUserEndpoints();

app.MapGet("/health", () => Results.Ok("IdentityService is running"));

// ---------------------------
// Run
// ---------------------------
app.Run();
