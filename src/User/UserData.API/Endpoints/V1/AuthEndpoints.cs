using Core.SharedKernel.Data.Enums;
using Core.SharedKernel.DTO;
using Core.SharedKernel.Exceptions;
using MediatR;
using UserData.Application.Handlers.CommandHandlers;
using UserData.Application.Handlers.Commands;

namespace UserData.API.Endpoints.V1;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        // 1.0
        var users = app.NewVersionedApi("Users");
        var usersV1 = users.MapGroup("/api/user")
                             .HasDeprecatedApiVersion(0.9)
                             .HasApiVersion(1.0);

        // POST /v1/create HTTP/1.1
        usersV1.MapPost("user", CreateUserForAuth())
            .Accepts<AuthTokenDTO>("application/json")
            .Produces<AuthTokenDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .MapToApiVersion(1.0)
            .WithName("CreateUser")
            .WithOpenApi(generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "The ID associated with the created Todo";
                return generatedOperation;
            });


        // POST /v1/token HTTP/1.1
        usersV1.MapPost("token", ValidateToken())
            .Accepts<AuthTokenDTO>("application/json")
            .Produces<AuthTokenDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .MapToApiVersion(1.0)
            .WithName("ValidateUser")
            .WithOpenApi(generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "The ID associated with the created Todo";
                return generatedOperation;
            });



        usersV1.MapGet("/", () =>
                     new UserDTO[]
                     {
                         new(){ EmailAddress = "john@example.com", Username = "JohnDoe", Role = UserRole.Owner, Place = "Some Place", ImageUrl = "image_url_1" },
                         new(){ EmailAddress = "john@example.com", Username = "JohnDoe", Role = UserRole.Admin, Place = "Some Place", ImageUrl = "image_url_1" },
                         new(){ EmailAddress = "jane@example.com", Username = "JaneDoe", Role = UserRole.Base, Place = "Yet Another Place", ImageUrl = "image_url_3" },
                     })
                .Produces<IEnumerable<UserDTO>>();

        usersV1.MapGet("/{id:int}", (int id) => new UserDTO() { EmailAddress = "john@example.com", Username = "JohnDoe", Role = UserRole.Owner, Place = "Some Place", ImageUrl = "image_url_1" })
                .Produces<UserDTO>()
                .Produces(404);

        usersV1.MapPost("/", (HttpRequest request, UserDTO user) =>
                    {
                        var scheme = request.Scheme;
                        var host = request.Host;
                        var location = new Uri($"{scheme}{Uri.SchemeDelimiter}{host}/api/users/{Guid.NewGuid()}");
                        return Results.Created(location, user);
                    })
                .Accepts<UserDTO>("application/json")
                .Produces<UserDTO>(201)
                .Produces(400);

        usersV1.MapDelete("/{id:int}", (int id) => Results.NoContent())
                .Produces(204);

    }

    private static Func<ValidateUserDTO, IMediator, Task<IResult>> ValidateToken()
    {
        return async (ValidateUserDTO model, IMediator mediator) =>
        {
            try
            {
                var request = new ValidateUserCommand(model);
                var result = await mediator.Send(request);
                return Results.Ok(result);
            }
            catch (InvalidRequestBodyException ex)
            {
                return Results.BadRequest(ResultDTO.Failure(ex.Errors));
            }
            catch (EntityNotFoundException ex)
            {
                return Results.UnprocessableEntity(ResultDTO.Failure(new string[] { ex.Message }));
            }
        };
    }

    private static Func<CreateOrUpdateUserDTO, IMediator, Task<IResult>> CreateUserForAuth()
    {
        return async (CreateOrUpdateUserDTO model, IMediator mediator) =>
        {
            try
            {
                var request = new CreateUserCommandHandler(model);
                var result = await mediator.Send(request);
                return TypedResults.Created($"/v1/users/{model.Username}", result);
            }
            catch (InvalidRequestBodyException ex)
            {
                return Results.BadRequest(ResultDTO.Failure(ex.Errors));
            }
        };
    }
}
