using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Routing;
namespace DreamData.Presentation.Dreams;
public static class DreamsModule
{
    public static void AddRoutes(WebApplication app)
    {
        var grp = app.MapGroup("dreams")
            .RequireAuthorization();

        grp.MapPost("", async (CreateDreams));

        grp.MapGet("", async (ISender sender) =>
        Results < List < DreamResponse >> result = await sender.Send
        ).WithName("GetDreams");

        grp.MapGet("{id:guid}", async (
            Guid id,
            ISender sender) =>)
            .WithName("GetDream");

        grp.MapPut("{id:guid}", async (
           Guid id,
           ISender sender) =>)
           .WithName("UpdateDream");

        grp.MapPut("{id:guid}", async (
          Guid id,
          ISender sender) =>)
          .WithName("DeleteDream");

    }
}
