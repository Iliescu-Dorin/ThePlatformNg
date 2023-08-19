using Core.SharedKernel.Constants;
using Core.SharedKernel.DTO.APICall;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Authentication.Application.Handlers;

public class ApiAuthorizationResultHandler : IAuthorizationMiddlewareResultHandler
{
    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        var response = context.Response;

        if (authorizeResult.Challenged)
        {
            response.Headers.Append(
                HeaderNames.WWWAuthenticate,
                JwtBearerDefaults.AuthenticationScheme
            );
            response.StatusCode = StatusCodes.Status401Unauthorized;

            var unauthorized = ApiResponse<object>.Error(ErrorCodes.Unauthorized);
            await response.WriteAsJsonAsync(unauthorized);

            return;
        }

        if (authorizeResult.Forbidden)
        {
            response.StatusCode = StatusCodes.Status403Forbidden;

            var forbidden = ApiResponse<object>.Error(ErrorCodes.Forbidden);
            await response.WriteAsJsonAsync(forbidden);

            return;
        }

        await next(context);
    }
}
