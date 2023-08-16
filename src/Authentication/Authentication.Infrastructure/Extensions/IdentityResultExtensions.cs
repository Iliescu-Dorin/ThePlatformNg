using Core.SharedKernel.DTO;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Infrastructure.Extensions;
public static class IdentityResultExtensions
{
    public static ResultDTO MapToResult(this IdentityResult result)
    {
        return result.Succeeded
            ? ResultDTO.Success()
            : ResultDTO.Failure(result.Errors.Select(e => e.Description));
    }

    public static ResultDTO MapToResult(this SignInResult result)
    {
        return result.Succeeded
            ? ResultDTO.Success()
            : ResultDTO.Failure(new string[] { "Invalid Login Attempt." });
    }
}
