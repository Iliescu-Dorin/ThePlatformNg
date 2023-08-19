using DreamData.Application.Handlers.Commands;
using DreamData.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace DreamData.Application.Services;
// This class implements a rather crude modular configuration of subcomponents, without any ceremony or meta-structure.
// Proper abstractions can be added later if modularization would seem to benefit from them.

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateDreamCommand>, CreateDreamCommandValidator>();

        return services;
    }
};
