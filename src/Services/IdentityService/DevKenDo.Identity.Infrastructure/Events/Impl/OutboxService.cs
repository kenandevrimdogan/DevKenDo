using DevKenDo.Identity.Infrastructure.Db;
using DevKenDo.Identity.Infrastructure.Db.Entities;
using DevKenDo.Identity.Infrastructure.Events.Infrastructure;

namespace DevKenDo.Identity.Infrastructure.Events.Impl;

public class OutboxService: IOutboxService
{
    private readonly IdentityDbContext _dbContext;

    public OutboxService(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveEventAsync(IEvent @event)
    {
        await _dbContext.OutboxEvents.AddAsync(new OutboxEvent
        {
            Id = @event.Id,
            CreatedAt = @event.CreatedAt,
            EventType = @event.GetType().Name,
            Payload = System.Text.Json.JsonSerializer.Serialize(@event)
        });
        await _dbContext.SaveChangesAsync();
    }
}