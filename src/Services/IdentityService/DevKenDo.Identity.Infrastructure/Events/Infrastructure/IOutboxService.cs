namespace DevKenDo.Identity.Infrastructure.Events.Infrastructure;

public interface IOutboxService
{
    Task SaveEventAsync(IEvent @event);
}