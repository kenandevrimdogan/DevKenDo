using DevKenDo.Identity.Application.Pipelines.Infrastructure;
using DevKenDo.Identity.Domain.Entities;
using MediatR;

namespace DevKenDo.Identity.Application.Queries.GetUserByEmail;

public class GetUserByEmailQuery : IRequest<User>, ICorrelatable
{
    public string Email { get; set; }
    public string CorrelationId { get; set; }
}