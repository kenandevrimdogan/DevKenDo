using DevKenDo.Identity.API.Models;
using DevKenDo.Identity.Application.Commands.CreateUser;
using MediatR;

namespace DevKenDo.Identity.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapPost("/users", async (CreateUserCommand command, IMediator mediator) =>
        {
            try
            {
                var userId = await mediator.Send(command);
                return Results.Ok(ApiResponse<Guid>.Ok(userId, "User created successfully"));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
        });
    }
}