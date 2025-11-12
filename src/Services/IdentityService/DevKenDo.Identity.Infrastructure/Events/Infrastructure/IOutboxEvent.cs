namespace DevKenDo.Identity.Infrastructure.Events.Infrastructure;

public interface IOutboxEvent : IEvent
{
    string EventType { get; }
    string Payload { get; }
}