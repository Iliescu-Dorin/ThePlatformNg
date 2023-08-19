using FluentValidation.Results;

namespace DreamData.Application.Extensions;

public static class ValidationErrorExtensions
{
    public static string GetErrors(this List<ValidationFailure> errors)
    {
        var errorMessages = "";
        errors.ForEach(err => errorMessages += err.ErrorMessage + " ");

        return errorMessages;
    }
}
