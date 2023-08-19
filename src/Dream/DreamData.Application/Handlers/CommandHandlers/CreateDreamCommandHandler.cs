using AutoMapper;
using Core.SharedKernel.DTO;
using Core.SharedKernel.Exceptions;
using DreamData.Application.Handlers.Commands;
using DreamData.Domain.Entities;
using DreamData.Infrastructure.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DreamData.Application.Handlers.CommandHandlers;

public class CreateItemCommandHandler : IRequestHandler<CreateDreamCommand, DreamDTO>
{
    private readonly IUnitOfWork _repository;
    private readonly IValidator<CreateOrUpdateDreamDTO> _validator;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateItemCommandHandler> _logger;

    public CreateItemCommandHandler(ILogger<CreateItemCommandHandler> logger, IUnitOfWork repository, IValidator<CreateOrUpdateDreamDTO> validator, IMapper mapper)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<DreamDTO> Handle(CreateDreamCommand request, CancellationToken cancellationToken)
    {
        CreateOrUpdateDreamDTO model = request.Model;

        var result = _validator.Validate(model);

        _logger.LogInformation($"Create Item Validation result: {result}");

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(x => x.ErrorMessage).ToArray();
            throw new InvalidRequestBodyException
            {
                Errors = errors
            };
        }

        var entity = new Dream(Guid.NewGuid())
        {
            UserId = model.UserId,
            Title = model.Title,
            Description = model.Description,
            Date = model.Date,
            Symbols = model.Symbols ?? new List<string>(), // Initialize with an empty list if null
            Interpretations = new List<Interpretation>() // Initialize with an empty list
        };

        _repository.Dreams.Add(entity);
        await _repository.CommitAsync();

        return _mapper.Map<DreamDTO>(entity);
    }
}
