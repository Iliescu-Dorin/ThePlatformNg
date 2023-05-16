using UserData.Domain.Entities;

namespace UserData.API.Services.Interfaces;

public interface IUserService
{
    public async Task<List<User>> GetAllUsers();
}
