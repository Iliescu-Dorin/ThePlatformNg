using DreamData.Application.Handlers.Commands;
using DreamData.Infrastructure.Interfaces;
using MediatR;

namespace DreamData.Application.Handlers.CommandHandlers;

public class DeleteItemCommandHandler : IRequestHandler<DeleteDreamCommand, int>
{
    private readonly IUnitOfWork _repository;

    public DeleteItemCommandHandler(IUnitOfWork repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(DeleteDreamCommand request, CancellationToken cancellationToken)
    {
        _repository.Dreams.Delete(request.DreamId);
        await _repository.CommitAsync();
        return request.DreamId;
    }
}
