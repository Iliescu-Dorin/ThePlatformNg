using Authentication.Domain;
using Core.Services.Authentication;
using Core.SharedKernel.DTO;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Application;
public class SignInManager : ISignInManager
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public SignInManager(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<ResultDTO> PasswordSignInAsync(string username, string password, bool isPersistent, bool LockoutOnFailiure)
    {
        var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent, LockoutOnFailiure);

        if (result.IsLockedOut)
        {
            return ResultDTO.Failure(new string[] { "Account Locked, too many invalid login attempts." });
        }

        return result.MapToResult();
    }
}
