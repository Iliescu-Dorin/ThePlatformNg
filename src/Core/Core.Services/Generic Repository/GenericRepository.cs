using Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UserData.Infrastructure.Repositories.Generic;
public class GenericRepository<T, TDbContext> : IRepository<T> where T : class where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(TDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> Get(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(object id)
    {
        var entity = await Get(id);
        if (entity != null)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> Count()
    {
        return await _dbSet.CountAsync();
    }
}
