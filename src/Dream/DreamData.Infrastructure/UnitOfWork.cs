using DreamData.Infrastructure.Interfaces;
using DreamData.Infrastructure.Repositories;

namespace DreamData.Infrastructure;
public class UnitOfWork : IUnitOfWork
{
    private readonly DreamDbContext _context;

    public UnitOfWork(DreamDbContext context)
    {
        _context = context;
    }

    //public IBlogRepository Items => new IBlogRepository(_context);

    public IDreamRepository Dreams => new DreamRepository(_context);

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}
