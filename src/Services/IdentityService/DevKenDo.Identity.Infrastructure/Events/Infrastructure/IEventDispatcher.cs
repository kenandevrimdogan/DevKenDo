namespace DevKenDo.Identity.Infrastructure.Events.Infrastructure;

public interface IEventDispatcher
{
    Task DispatchAsync(IEvent @event);
}