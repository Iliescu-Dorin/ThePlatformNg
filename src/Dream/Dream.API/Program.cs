
using DreamData.Infrastructure.Interfaces;
using DreamData.Infrastructure.Repositories;
using DreamData.Presentation.Dreams;

namespace DreamData.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //builder.Services.AddSingleton(IDreamRepository, DreamRepository);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        DreamsModule.AddRoutes(app);

        app.UseAuthorization();

        app.Run();
    }
}
