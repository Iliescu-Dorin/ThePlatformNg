using Core.SharedKernel.DTO;
using MediatR;

namespace DreamData.Application.Handlers.Queries;
public class GetAllDreamsQuery : IRequest<IEnumerable<DreamDTO>>
{
}
