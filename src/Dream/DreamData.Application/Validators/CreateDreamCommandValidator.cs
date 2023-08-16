using DreamData.Application.Handlers.Commands;
using FluentValidation;
using Microsoft.Extensions.Localization;


namespace DreamData.Application.Validators;
public class CreateDreamCommandValidator : AbstractValidator<CreateDreamCommand>
{
    private readonly IStringLocalizer<Messages> _localizer;

    public CreateDreamCommandValidator(IStringLocalizer<Messages> localizer)
    {
        _localizer = localizer;

        RuleFor(v => v.Model.Title)
            .NotEmpty().WithMessage(_localizer["PNameRequired"].Value);

        RuleFor(v => v.Model.Description)
            .NotEmpty().WithMessage(_localizer["UnitPriceRequired"].Value);

        RuleFor(v => v.Model.Symbols)
            .NotEmpty().WithMessage(_localizer["CategoryIdRequired"].Value);
    }
}
