using Yarp.ReverseProxy.Configuration;
using YARP.LoadBalancer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services
            .AddSingleton<IProxyConfigProvider>(new CustomProxyConfigProvider())
            .AddReverseProxy();
//.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapReverseProxy();
});

app.MapReverseProxy();

app.MapHealthChecks("health");

app.Run();
