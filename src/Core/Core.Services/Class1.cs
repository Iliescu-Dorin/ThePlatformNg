using Core.SharedKernel.Common.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
namespace Core.Services;

public class AzureKeyVaultSettings
{
    [RequiredIf(nameof(AddToConfiguration), true)]
    public string? ServiceUrl { get; init; }

    [MemberNotNullWhen(true, nameof(ServiceUrl))]
    public bool AddToConfiguration { get; init; }
}
