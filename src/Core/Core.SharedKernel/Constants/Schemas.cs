using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Core.SharedKernel.Constants;

public static class Schemes
{
    public const string SchemeSelector = "auth-scheme-selector";

    public const string JwtBearer = JwtBearerDefaults.AuthenticationScheme;

    public const string OpenApi = nameof(OpenApi);
}
