using Authentication.Domain.Policy;

namespace Core.Services.Authentication.Interfaces;

public interface IPermissionChecker
{
    bool IsGranted(IEnumerable<PolicyStatement> statements, PermissionRequirement requirement);
}
