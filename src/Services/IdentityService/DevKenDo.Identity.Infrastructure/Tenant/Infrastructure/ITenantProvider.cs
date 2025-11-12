namespace DevKenDo.Identity.Infrastructure.Tenant.Infrastructure;

public interface ITenantProvider
{
    Guid TenantId { get; }
}