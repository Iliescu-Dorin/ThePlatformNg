using MediatR;

namespace DreamData.Application.Handlers.Commands;

public class DeleteDreamCommand : IRequest<int>
{
    public int DreamId { get; }

    public DeleteDreamCommand(int dreamId)
    {
        DreamId = dreamId;
    }
}
