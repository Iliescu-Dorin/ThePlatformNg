using Core.SharedKernel.DTO;

namespace Core.Services.Authentication.Interfaces;

public interface ITokenService
{
    AuthTokenDTO Generate(UserDTO user);
}
