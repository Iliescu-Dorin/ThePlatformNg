using UserData.Domain.Entities;
using UserData.Infrastructure.Interfaces;
using UserData.Infrastructure.Repositories.Generic;

namespace UserData.Infrastructure.Repositories;
public class UserRepository : GenericRepository<User, UserDbContext>, IUserRepository
{
    public UserRepository(UserDbContext context) : base(context)
    {
    }
}
