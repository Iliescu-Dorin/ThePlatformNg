using UserData.Infrastructure.Interfaces;
using UserData.Infrastructure.Repositories;

namespace UserData.Infrastructure;
public class UnitOfWork : IUnitOfWork
{
    private readonly UserDbContext _context;

    public UnitOfWork(UserDbContext context)
    {
        _context = context;
    }

    //public IBlogRepository Items => new IBlogRepository(_context);

    public IUserRepository Users => new UserRepository(_context);

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}
