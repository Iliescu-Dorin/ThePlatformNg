namespace DreamData.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddOutputCache()
            .AddStackExchangeRedisOutputCache(x =>             // new Microsoft .NET 8 Redis Cache Extension https://www.youtube.com/watch?v=_bg5dGnudPs&ab_channel=NickChapsas
            {
                //x.ConnectionMultiplexerFactory = async () => await ConnectionMultiplexer.Connect("localhost:6379");
                x.InstanceName = "DreamsApi";
                x.Configuration = "localhost:6379";
            });

        //builder.Services.AddSingleton(IDreamRepository, DreamRepository);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.Run();
    }
}
