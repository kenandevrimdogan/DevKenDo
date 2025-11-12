namespace DevKenDo.Identity.Infrastructure.Events.Infrastructure;

public interface IEvent
{
    Guid Id { get; }
    DateTime CreatedAt { get; }
}
