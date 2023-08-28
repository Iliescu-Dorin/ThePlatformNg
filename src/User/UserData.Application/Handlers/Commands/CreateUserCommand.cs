using Core.SharedKernel.DTO;
using MediatR;

namespace UserData.Application.Handlers.Commands;

public class CreateUserCommand : IRequest<AuthTokenDTO>
{
    public CreateUserCommand(CreateOrUpdateUserDTO model)
    {
        Model = model;
    }

    public CreateOrUpdateUserDTO Model { get; }
}
