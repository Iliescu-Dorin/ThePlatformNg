using AutoMapper;
using Core.Services.Authentication.Interfaces;
using Core.SharedKernel.DTO;
using Core.SharedKernel.Exceptions;
using FluentValidation;
using MediatR;
using UserData.Application.Handlers.Commands;
using UserData.Domain.Entities;
using UserData.Infrastructure.Interfaces;

namespace UserData.Application.Handlers.CommandHandlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, AuthTokenDTO>
{
    private readonly IUnitOfWork _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateOrUpdateUserDTO> _validator;
    private readonly ITokenService _token;

    public CreateUserCommandHandler(IUnitOfWork repository, IValidator<CreateOrUpdateUserDTO> validator, ITokenService token, IMapper mapper)
    {
        _repository = repository;
        _validator = validator;
        _token = token;
        _mapper = mapper;
    }

    public async Task<AuthTokenDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        var result = _validator.Validate(model);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(x => x.ErrorMessage).ToArray();
            throw new InvalidRequestBodyException
            {
                Errors = errors
            };
        }

        var user = new User(Guid.NewGuid())
        {
            EmailAddress = model.EmailAddress,
            Username = model.Username ?? "",
            Password = model.Password,
            Role = model.Role,
            Place = model.Place ?? "",
            ImageUrl = model.ImageUrl ?? ""
        };

        _repository.Users.Add(user);
        await _repository.CommitAsync();

        var userDTO = _mapper.Map<User, UserDTO>(user);
        return _token.Generate(userDTO);
    }
}
