using UserData.Domain.Entities;
using UserData.Infrastructure.Interfaces;
using UserData.Infrastructure.Repositories.Generic;

namespace UserData.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User, UserDbContext>, IUserRepository
{
    // create context property
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context) : base(context)
    {
        _context = context;
    }
}
