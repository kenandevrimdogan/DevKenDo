using DevKenDo.Identity.Infrastructure.Events.Infrastructure;

namespace DevKenDo.Identity.Application.UseCases.UserCreatedEvent;

public record UserCreatedEvent(Guid UserId) : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}