using AutoMapper;
using Core.SharedKernel.DTO;
using DreamData.Application.Handlers.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace DreamData.Application.Handlers.QueryHandlers;

public class GetAllIDreamsQueryHandler : IRequestHandler<GetAllDreamsQuery, IEnumerable<DreamDTO>>
{
    private readonly IUnitOfWork _repository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;

    public GetAllIDreamsQueryHandler(IUnitOfWork repository, IMapper mapper, IDistributedCache cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<IEnumerable<DreamDTO>> Handle(GetAllDreamsQuery request, CancellationToken cancellationToken)
    {
        string? cachedEntitiesString = await _cache.GetStringAsync("all_items");

        if (cachedEntitiesString == null)
        {
            var entities = await Task.FromResult(_repository.Items.GetAll());
            IEnumerable<DreamDTO> result = _mapper.Map<IEnumerable<DreamDTO>>(entities);

            await _cache.SetStringAsync("all_items", JsonConvert.SerializeObject(result));
            return result;
        }
        else
        {
            IEnumerable<DreamDTO> cachedEntities = JsonConvert.DeserializeObject<IEnumerable<DreamDTO>>(cachedEntitiesString);
            return cachedEntities;
        }
    }
}
