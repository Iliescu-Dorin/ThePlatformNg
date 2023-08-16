using Core.Services.APIs.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
namespace Core.Services.APIs;
public class CurrentUserService : ICurrentUserService
{
    public string UserId { get; }
    public string? UserName { get; }
    public bool IsAuthenticated { get; }
    public string IpAddress { get; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        HttpContext? httpContext = httpContextAccessor.HttpContext;
        IpAddress = httpContext?.Connection?.RemoteIpAddress.ToString();
        UserName = httpContext.User?.FindFirstValue(ClaimTypes.Name);
        UserId = httpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        IsAuthenticated = UserId != null;
    }
}
