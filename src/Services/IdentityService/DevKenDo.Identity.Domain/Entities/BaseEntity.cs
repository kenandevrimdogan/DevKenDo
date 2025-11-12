namespace DevKenDo.Identity.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Guid TenantId { get; set; }
}