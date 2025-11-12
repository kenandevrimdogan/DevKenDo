using DevKenDo.Identity.Application.Pipelines.Infrastructure;
using MediatR;

namespace DevKenDo.Identity.Application.Commands.CreateUser;

public class CreateUserCommand : IRequest<Guid>, ICorrelatable
{
    public string Email { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }    
    public string PasswordHash { get; set; }   

    public string CorrelationId { get; set; }
}