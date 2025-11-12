using DevKenDo.Identity.Infrastructure.Events.Infrastructure;

namespace DevKenDo.Identity.Application.Commands.CreateUser;

public record UserCreatedEvent: IEvent
{
    public Guid Id { get; }
    public string Email { get; }
    public string FullName { get; }
    public string CorrelationId { get; }
    public DateTime CreatedAt { get; }

    public UserCreatedEvent(Guid userId, string email, string fullName, string correlationId, DateTime createdAt)
    {
        Id = userId;
        Email = email;
        FullName = fullName;
        CorrelationId = correlationId;
        CreatedAt = createdAt;
    }
}