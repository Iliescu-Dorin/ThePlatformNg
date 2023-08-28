using Core.SharedKernel.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UserData.Domain.Entities;
using UserData.Infrastructure;

namespace UserData.API.Endpoints.V1;

public static class UsersEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/users", List)
            .WithTags("UsersGroup");

        app.MapGet("/users/{id}", Get)
            .WithTags("UsersGroup");

        app.MapGet("/users",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        (UserDbContext db, User user) => Create)
            .Produces<UserDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("CreateUser")
            .WithTags("UsersGroup")
            .Accepts<UserDTO>("application/json")
            .WithOpenApi(generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "The ID associated with the created User";
                return generatedOperation;
            });

        app.MapGet("/users", Update)
            .WithTags("UsersGroup");

        app.MapGet("/users/{id}", Delete)
            .WithTags("UsersGroup")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "This is a summary about the Deletion of an User",
                Description = "This is a description about the  Deletion of an User"
            });
    }

    public static async Task<IResult> List(UserDbContext db)
    {
        var result = await db.Users.ToListAsync();
        return TypedResults.Ok(result);
    }

    //best example of minimal API
    public static async Task<Results<Ok<User>, NotFound>> Get(UserDbContext db, string id)
    {
        return await db.Users.FindAsync(id) is User user
            ? TypedResults.Ok(user)
            : TypedResults.NotFound();
    }

    public static async Task<IResult> Create(UserDbContext db, User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/users/{user.Id}", user);
    }

    public static async Task<IResult> Update(UserDbContext db, User updatedUser)
    {
        var user = await db.Users.FindAsync(updatedUser);

        if (user is null) return TypedResults.NotFound();

        user.Id = updatedUser.Id;
        user.Username = updatedUser.Username;
        user.Place = updatedUser.Place;
        user.ImageUrl = updatedUser.ImageUrl;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public static async Task<IResult> Delete(UserDbContext db, int id)
    {
        if (await db.Users.FindAsync(id) is User user)
        {
            db.Users.Remove(user);
            await db.SaveChangesAsync(true);
            return TypedResults.Ok(user);
        }
        return TypedResults.NotFound();
    }
}
