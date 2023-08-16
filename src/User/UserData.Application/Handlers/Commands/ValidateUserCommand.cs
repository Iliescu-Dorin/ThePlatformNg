using Core.SharedKernel.DTO;
using MediatR;

namespace UserData.Application.Handlers.Commands;

public class ValidateUserCommand : IRequest<AuthTokenDTO>
{
    public ValidateUserCommand(ValidateUserDTO model)
    {
        Model = model;
    }

    public ValidateUserDTO Model { get; }
}
