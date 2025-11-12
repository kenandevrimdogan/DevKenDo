using DevKenDo.Identity.Domain.Entities;
using DevKenDo.Identity.Infrastructure.Db;
using DevKenDo.Identity.Infrastructure.Events.Infrastructure;
using MediatR;

namespace DevKenDo.Identity.Application.Commands.CreateUser;

public class CreateUserCommandHandler: IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IdentityDbContext _dbContext;
    private readonly IEventDispatcher _eventDispatcher;

    public CreateUserCommandHandler(IdentityDbContext dbContext, IEventDispatcher eventDispatcher)
    {
        _dbContext = dbContext;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FullName = request.FullName,
            UserName = request.UserName,
            PasswordHash = request.PasswordHash,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Users.AddAsync(user, cancellationToken);

        var userCreatedEvent = new UserCreatedEvent(
            user.Id,
            user.Email,
            user.FullName,
            request.CorrelationId,
            DateTime.UtcNow
        );
        await _eventDispatcher.DispatchAsync(userCreatedEvent);

        await _dbContext.SaveChangesAsync(cancellationToken);
        // await transaction.CommitAsync(cancellationToken);

        return user.Id;
    }
}
