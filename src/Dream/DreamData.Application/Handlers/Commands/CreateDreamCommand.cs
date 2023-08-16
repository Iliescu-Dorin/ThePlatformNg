using Core.SharedKernel.DTO;
using MediatR;

namespace DreamData.Application.Handlers.Commands;

public class CreateDreamCommand : IRequest<DreamDTO>
{
    public CreateOrUpdateDreamDTO Model { get; }
    public CreateDreamCommand(CreateOrUpdateDreamDTO model)
    {
        this.Model = model;
    }
}
