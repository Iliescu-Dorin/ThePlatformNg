using Core.SharedKernel.DTO;
using MediatR;

namespace DreamData.Application.Handlers.Queries;
public class GetDreamByIdQuery : IRequest<DreamDTO>
{
    public int ItemId { get; }
    public GetDreamByIdQuery(int id)
    {
        ItemId = id;
    }
}
