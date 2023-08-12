using DreamData.Domain.Entities;
using DreamData.Infrastructure.Interfaces;

namespace DreamData.Infrastructure.Repositories;
public class DreamRepository : IDreamDataRepository
{
    public Task AddDream(Dream dream)
    {
        throw new NotImplementedException();
    }

    public Task<List<Dream>> GetAllDreams()
    {
        throw new NotImplementedException();
    }

    public Task<Dream> GetDreamById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Dream>> GetDreamsByCulture(string culture)
    {
        throw new NotImplementedException();
    }
}
