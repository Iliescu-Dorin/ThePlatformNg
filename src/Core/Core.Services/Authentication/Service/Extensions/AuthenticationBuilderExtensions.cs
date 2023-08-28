using Authentication.Application.Handlers;
using Authentication.Application.Options;
using Microsoft.AspNetCore.Authentication;

namespace Authentication.Application.Extensions;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddOpenApi(this AuthenticationBuilder builder, string scheme)
    {
        return builder.AddScheme<OpenApiOptions, OpenApiHandler>(scheme, scheme, _ => { });
    }
}
