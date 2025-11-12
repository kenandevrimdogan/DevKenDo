using DevKenDo.Identity.Infrastructure.Events.Infrastructure;

namespace DevKenDo.Identity.Infrastructure.Db.Entities;

public class OutboxEvent : IOutboxEvent
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string EventType { get; set; }
    public string Payload { get; set; }
}