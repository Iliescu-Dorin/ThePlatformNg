using Core.SharedKernel.DTO.APICall;

namespace Core.Services.Authentication.Interfaces;

public interface ISignInManager
{
    Task<ResultDTO> PasswordSignInAsync(string username, string password, bool isPersistent, bool LockoutOnFailiure);
}
