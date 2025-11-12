using DevKenDo.Identity.Infrastructure.Tenant.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace DevKenDo.Identity.Infrastructure.Tenant.Impl;

public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid TenantId
    {
        get
        {
            var tenantHeader = _httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-ID"].FirstOrDefault();
            return Guid.TryParse(tenantHeader, out var tenantId) ? tenantId : Guid.Empty;
        }
    }
}