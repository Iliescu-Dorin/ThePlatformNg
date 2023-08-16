using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services;
public class UserService 
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;
            if (context?.User != null && context?.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                var identifier = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (identifier != null)
                {
                    return identifier.Value;
                }
            }

            return string.Empty;
        }
    }
}
