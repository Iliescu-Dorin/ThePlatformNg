using AutoMapper;
using Core.Services.Interfaces;
using Core.SharedKernel.DTO;
using Core.SharedKernel.Exceptions;
using DreamData.Application.Handlers.Commands;
using DreamData.Domain.Entities;
using DreamData.Infrastructure.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DreamData.Application.Handlers.CommandHandlers;

public class UpdateDreamCommandHandler : IRequestHandler<UpdateDreamCommand, DreamDTO>
{
    private readonly IUnitOfWork _repository;
    private readonly IValidator<CreateOrUpdateDreamDTO> _validator;
    private readonly IMapper _mapper;
    private readonly ICachingService _cache;
    private readonly ILogger<UpdateDreamCommandHandler> _logger;

    public UpdateDreamCommandHandler(ILogger<UpdateDreamCommandHandler> logger, IUnitOfWork repository, IValidator<CreateOrUpdateDreamDTO> validator, IMapper mapper, ICachingService cache)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
        _cache = cache;
        _logger = logger;
    }

    async Task<DreamDTO> IRequestHandler<UpdateDreamCommand, DreamDTO>.Handle(UpdateDreamCommand request, CancellationToken cancellationToken)
    {
        CreateOrUpdateDreamDTO model = request.Model;
        int dreamId = request.DreamId;

        ValidateModel(model);

        Dream dbEntity = GetDreamEntity(dreamId);

        await UpdateDreamEntityFromModel(dbEntity, model);

        await UpdateDreamInRepository(dbEntity);

        await UpdateCachedItemIfPresent(dreamId, dbEntity);

        return _mapper.Map<DreamDTO>(dbEntity);
    }

    private void ValidateModel(CreateOrUpdateDreamDTO model)
    {
        var result = _validator.Validate(model);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(x => x.ErrorMessage).ToArray();
            throw new InvalidRequestBodyException { Errors = errors };
        }
    }

    private Dream GetDreamEntity(int dreamId)
    {
        var dbEntity = _repository.Dreams.Get(dreamId);
        if (dbEntity == null)
        {
            throw new EntityNotFoundException($"No Item found for the Id {dreamId}");
        }
        return dbEntity;
    }

    private void UpdateDreamEntityFromModel(Dream dbEntity, CreateOrUpdateDreamDTO model)
    {
        dbEntity.UserId = model.UserId;
        dbEntity.Title = model.Title;
        dbEntity.Description = model.Description;
        dbEntity.Date = model.Date;
        dbEntity.Symbols = model.Symbols ?? new List<string>();
        dbEntity.Interpretations = new List<Interpretation>();
    }

    private void UpdateDreamInRepository(Dream dbEntity)
    {
        _repository.Dreams.Update(dbEntity);
        _repository.CommitAsync().Wait(); // Blocking here might need further optimization
    }

    private void UpdateCachedItemIfPresent(int dreamId, Dream dbEntity)
    {
        if (_cache.GetItem<DreamDTO>($"item_{dreamId}") != null)
        {
            _logger.LogInformation($"Item Exists in Cache. Set new Item for the same Key.");
            _cache.SetItem($"item_{dreamId}", _mapper.Map<DreamDTO>(dbEntity));
        }
    }

}
