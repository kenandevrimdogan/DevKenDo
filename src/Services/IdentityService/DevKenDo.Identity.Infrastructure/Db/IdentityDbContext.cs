using Microsoft.EntityFrameworkCore;   
using DevKenDo.Identity.Domain.Entities;
using DevKenDo.Identity.Infrastructure.Db.Entities;
using DevKenDo.Identity.Infrastructure.Tenant.Infrastructure;

namespace DevKenDo.Identity.Infrastructure.Db;

public class IdentityDbContext : DbContext
{
    private readonly ITenantProvider _tenantProvider;

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options, ITenantProvider tenantProvider)
        : base(options)
    {
        _tenantProvider = tenantProvider;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasQueryFilter(u => u.TenantId == _tenantProvider.TenantId);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<OutboxEvent> OutboxEvents { get; set; }

}