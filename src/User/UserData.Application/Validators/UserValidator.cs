using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserData.Domain.Entities;

namespace UserData.Application.Validators;
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(o => o.Id).NotNull().NotEmpty();
        RuleFor(o => o.Username).NotNull().NotEmpty().MinimumLength(5);
        RuleFor(o => o.Place).NotNull().NotEmpty();
        RuleFor(o => o.ImageUrl).NotNull().NotEmpty();
    }
}
