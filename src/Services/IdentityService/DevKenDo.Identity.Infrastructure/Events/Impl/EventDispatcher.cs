using DevKenDo.Identity.Infrastructure.Events.Infrastructure;

namespace DevKenDo.Identity.Infrastructure.Events.Impl;

public class EventDispatcher : IEventDispatcher
{
    private readonly IOutboxService _outboxService;

    public EventDispatcher(IOutboxService outboxService)
    {
        _outboxService = outboxService;
    }

    public async Task DispatchAsync(IEvent @event)
    {
        await _outboxService.SaveEventAsync(@event);
    }
}
