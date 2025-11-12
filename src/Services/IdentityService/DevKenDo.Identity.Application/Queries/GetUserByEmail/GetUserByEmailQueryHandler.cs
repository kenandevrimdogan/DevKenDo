using DevKenDo.Identity.Domain.Entities;
using DevKenDo.Identity.Infrastructure.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevKenDo.Identity.Application.Queries.GetUserByEmail;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, User>
{
    private readonly IdentityDbContext _dbContext;

    public GetUserByEmailQueryHandler(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
    }
}