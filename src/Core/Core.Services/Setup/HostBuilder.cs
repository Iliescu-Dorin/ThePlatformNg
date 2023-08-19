using Microsoft.AspNetCore.Builder;

namespace Core.SharedKernel.Setup;

public static class HostBuilder
{
    public static WebApplicationBuilder ConfigureHost(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog();

        return builder;
    }
}
