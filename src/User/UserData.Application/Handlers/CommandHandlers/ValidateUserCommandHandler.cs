using Core.Services.Authentication.Interfaces;
using Core.SharedKernel.DTO;
using Core.SharedKernel.Exceptions;
using FluentValidation;
using MediatR;
using UserData.Application.Handlers.Commands;
using UserData.Infrastructure.Interfaces;

namespace UserData.Application.Handlers.CommandHandlers;

public class ValidateUserCommandHandler : IRequestHandler<ValidateUserCommand, AuthTokenDTO>
{
    private readonly IUnitOfWork _repository;
    private readonly IValidator<ValidateUserDTO> _validator;
    private readonly ITokenService _token;

    public ValidateUserCommandHandler(IUnitOfWork repository, IValidator<ValidateUserDTO> validator, ITokenService token)
    {
        _repository = repository;
        _validator = validator;
        _token = token;
    }

    public async Task<AuthTokenDTO> Handle(ValidateUserCommand request, CancellationToken cancellationToken)
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

        var entities = _repository.Users.GetAll().Where(x => x.EmailAddress == model.EmailAddress);
        if (!entities.Any()) throw new EntityNotFoundException($"No Users matching emailAddress {model.EmailAddress} found");

        var user = entities.Where(x => x.Password == model.Password).FirstOrDefault();
        if (user == null) throw new EntityNotFoundException($"Passwords do not match. Authentication Failed.");

        return await Task.FromResult(_token.Generate(user));
    }
}
