using Core.SharedKernel.DTO;
using FluentValidation;

namespace UserData.Application.Validators;
public class ValidateUserDTOValidator : AbstractValidator<ValidateUserDTO>
{
    public ValidateUserDTOValidator()
    {
        RuleFor(x => x.EmailAddress).NotEmpty().WithMessage("EmailAddress is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        RuleFor(x => x.EmailAddress).EmailAddress().WithMessage("Not a valid EmailAddress");
    }
}
