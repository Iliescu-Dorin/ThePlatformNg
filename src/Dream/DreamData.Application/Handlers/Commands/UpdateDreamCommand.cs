using MediatR;
using Core.SharedKernel.DTO;

namespace DreamData.Application.Handlers.Commands;

public class UpdateDreamCommand : IRequest<DreamDTO>
{
    public int DreamId { get; set; }
    public CreateOrUpdateDreamDTO Model { get; }

    public UpdateDreamCommand(int DreamId, CreateOrUpdateDreamDTO model)
    {
        this.DreamId = DreamId;
        this.Model = model;
    }
}
