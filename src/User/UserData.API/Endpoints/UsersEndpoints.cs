using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using UserData.Domain.Entities;
using UserData.Infrastructure;

namespace UserData.API.Endpoints;

public static class UsersEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/users", List);
        app.MapGet("/users/{id}", Get);
        app.MapGet("/users", Create);
        app.MapGet("/users", Update);
        app.MapGet("/users/{id}", Delete);

    }

    public static async Task<IResult> List(UserDbContext db)
    {
        var result = await db.Users.ToListAsync();
        return Results.Ok(result);
    }

    public static async Task<IResult> Get(UserDbContext db, string id)
    {
        return await db.Users.FindAsync(id) is User user
            ? Results.Ok(user)
            : Results.NotFound();
    }

    public static async Task<IResult> Create(UserDbContext db, User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return Results.Created($"/users/{user.Id}", user);
    }

    public static async Task<IResult> Update(UserDbContext db, User updatedUser)
    {
        var user = await db.Users.FindAsync(updatedUser);

        if (user is null) return Results.NotFound();

        user.Id = updatedUser.Id;
        user.Username = updatedUser.Username;
        user.Place = updatedUser.Place;
        user.ImageUrl = updatedUser.ImageUrl;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    public static async Task<IResult> Delete(UserDbContext db, int id)
    {
        if (await db.Users.FindAsync(id) is User user)
        {
            db.Users.Remove(user);
            await db.SaveChangesAsync(true);
            return Results.Ok(user);
        }
        return Results.NotFound();
    }
}
