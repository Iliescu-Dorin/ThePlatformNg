using Core.SharedKernel.DTO;
using Core.SharedKernel.DTO.APICall;
using Core.SharedKernel.Exceptions;
using DreamData.Application.Handlers.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

public static class DreamsEndpoints
{
    public static void MapDreamsEndpoints(this WebApplication app)
    {
        var dreams = app.NewVersionedApi("Dreams");
        var dreamsV1 = dreams.MapGroup("/api/dreams")
                            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
                            .HasDeprecatedApiVersion(0.9)
                            .HasApiVersion(1.0)
                            .RequireAuthorization();

        dreamsV1.MapPost("", Create)
            .WithName("CreateDream")
            .Accepts<DreamDTO>("application/json")
            .Produces<DreamDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .MapToApiVersion(1.0)
            .WithOpenApi(generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "The ID associated with the created Dream";
                return generatedOperation;
            });

        dreamsV1.MapGet("", () =>
                     new DreamDTO[]
                     {
                         new(){ Title = "Lucid Dreaming", Description = "Exploring the world of lucid dreams.", Date = DateTime.UtcNow },
                         new(){ Title = "Flying High", Description = "Soaring through the sky in dreams.", Date = DateTime.UtcNow },
                         new(){ Title = "Underwater Adventures", Description = "Diving into deep oceans during dreams.", Date = DateTime.UtcNow },
                     })
                .CacheOutput(x => x.Expire(TimeSpan.FromMinutes(5)).Tag("Dreams")) // new Microsoft .NET 8 Redis Cache Extension
                .Produces<IEnumerable<DreamDTO>>();

        dreamsV1.MapGet("inval", async (IOutputCacheStore store) =>
        {
            await store.EvictByTagAsync("Dreams", CancellationToken.None);
            return Results.Ok();
        });

        dreamsV1.MapGet("{id:int}", (int id) => new DreamDTO() { Title = "Lucid Dreaming", Description = "Exploring the world of lucid dreams.", Date = DateTime.UtcNow })
                .Produces<DreamDTO>()
                .WithName("GetDreamById")
                .Produces<DreamDTO>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status404NotFound);

        dreamsV1.MapPost("", (HttpRequest request, DreamDTO dream) =>
        {
            var scheme = request.Scheme;
            var host = request.Host;
            var location = new Uri($"{scheme}{Uri.SchemeDelimiter}{host}/api/dreams/{Guid.NewGuid()}");
            return Results.Created(location, dream);
        })
                .Accepts<DreamDTO>("application/json")
                .Produces<DreamDTO>(201)
                .Produces(400);

        dreamsV1.MapDelete("{id:int}", (int id) => Results.NoContent())
                .WithName("DeleteDreamWithId")
                .Produces(204);
    }

    private static Func<ValidateDreamDTO, IMediator, Task<IResult>> ValidateDream()
    {
        return async (ValidateDreamDTO model, IMediator mediator) =>
        {
            try
            {
                var request = new ValidateDreamCommand(model);
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

    // Most Updated
    private static async Task<IResult> Create([FromBody] CreateOrUpdateDreamDTO model, IMediator mediator)
    {
        try
        {
            var request = new CreateDreamCommand(model);
            var result = await mediator.Send(request);
            return TypedResults.Created($"/v1/dreams/{model.Title}", result);
        }
        catch (InvalidRequestBodyException ex)
        {
            return Results.BadRequest(ResultDTO.Failure(ex.Errors));
        }
    }
}
