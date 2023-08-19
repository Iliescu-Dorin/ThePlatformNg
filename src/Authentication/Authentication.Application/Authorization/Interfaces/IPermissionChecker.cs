using Authentication.Domain.Policy;

namespace Authentication.Application.Authorization.Interfaces;

public interface IPermissionChecker
{
    bool IsGranted(IEnumerable<PolicyStatement> statements, PermissionRequirement requirement);
}
