namespace UserData.Application.Validators;
using FluentValidation;
using UserData.Application.Handlers.Commands;

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(v => v.Username)
            .NotNull()
            .NotEmpty().WithMessage("Username field is required.");

        RuleFor(v => v.Password)
            .NotNull()
            .NotEmpty().WithMessage("Password field is required.");
    }
}
