namespace Authentication.Application.Interfaces;

public interface IAccessTokenService : IService<AccessToken>
{
    Task<PagedResult<AccessToken>> GetListAsync(Guid organizationId, AccessTokenFilter filter);

    Task DeleteAsync(Guid id);
}
