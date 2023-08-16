using Core.Services.Authentication;
using MediatR;

namespace UserData.Application.Handlers.Commands;
public class SignInCommand : IRequest<AuthResult>
{
    public string Username { get; set; }
    public string Password { get; set; }
}
