using AutoMapper;
using Core.Services.Interfaces;
using Core.SharedKernel.DTO;
using Core.SharedKernel.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DreamData.Application.Handlers.Queries;
public class GetDreamByIdQueryHandler : IRequestHandler<GetDreamByIdQuery, DreamDTO>
{
    private readonly IUnitOfWork _repository;
    private readonly IMapper _mapper;
    private readonly ICachingService _cache;
    private readonly ILogger<GetDreamByIdQueryHandler> _logger;

    public GetDreamByIdQueryHandler(ILogger<GetDreamByIdQueryHandler> logger, IUnitOfWork repository, IMapper mapper, ICachingService cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
        _logger = logger;
    }

    public async Task<DreamDTO> Handle(GetDreamByIdQuery request, CancellationToken cancellationToken)
    {
        // Check if the item is in the cache
        DreamDTO cachedItem = _cache.GetItem<DreamDTO>($"item_{request.ItemId}");
        if (cachedItem != null)
        {
            _logger.LogInformation($"Item Exists in Cache. Return CachedItem.");
            return cachedItem;
        }

        _logger.LogInformation($"Item doesn't exist in Cache.");

        // Fetch the item from the repository
        var item = await Task.FromResult(_repository.Items.Get(request.ItemId))
            ?? throw new EntityNotFoundException($"No Item found for Id {request.ItemId}");

        // Map the item to a DTO
        DreamDTO result = _mapper.Map<DreamDTO>(item);

        // Cache the item & return
        _logger.LogInformation($"Add Item to Cache and return.");
        DreamDTO _ = _cache.SetItem($"item_{request.ItemId}", result);

        return result;
    }
}
