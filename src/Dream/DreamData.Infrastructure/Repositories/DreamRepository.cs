using DreamData.Domain.Entities;
using DreamData.Infrastructure.Interfaces;
using UserData.Infrastructure.Repositories.Generic;

namespace DreamData.Infrastructure.Repositories;
public class DreamRepository : GenericRepository<Dream, DreamDbContext>, IDreamRepository
{
    public DreamRepository(DreamDbContext context) : base(context)
    {
    }

    public Task<List<Dream>> GetDreamsByCulture(string culture)
    {
        throw new NotImplementedException();
    }
}

