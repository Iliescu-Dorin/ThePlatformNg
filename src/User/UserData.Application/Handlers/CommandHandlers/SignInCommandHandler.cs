using Authentication.Application.Interfaces;
using Core.Services.Authentication;
using Core.Services.Authentication.Interfaces;
using Core.Services.Authentication.Oauth;
using Core.SharedKernel.Exceptions;
using MediatR;
using UserData.Application.Handlers.Commands;

namespace UserData.Application.Handlers.CommandHandlers;

public class SignInCommandHandler : IRequestHandler<SignInCommand, AuthResult>
{
    private readonly ISignInManager _signInManager;
    private readonly IJwtTokenManager _jwtTokenManager;

    public SignInCommandHandler(ISignInManager signInManager, JwtTokenManager jwtTokenManager)
    {
        _signInManager = signInManager;
        _jwtTokenManager = jwtTokenManager;
    }

    public async Task<AuthResult> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        // validate username & password
        var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);

        // Throw exception if credential validation failed
        if (!result.Succeeded)
        {
            throw new UnauthorizedException("Invalid username or password.");
        }

        // Generate JWT token response if validation successful
        AuthResult response = await _jwtTokenManager.GenerateClaimsTokenAsync(request.Username, cancellationToken);

        return response;
    }
}
