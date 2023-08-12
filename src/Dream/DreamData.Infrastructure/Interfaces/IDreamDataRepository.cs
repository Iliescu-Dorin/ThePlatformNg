using DreamData.Domain.Entities;

namespace DreamData.Infrastructure.Interfaces;
public interface IDreamDataRepository
{
    Task AddDream(Dream dream);
    Task<Dream> GetDreamById(Guid id);
    Task<List<Dream>> GetAllDreams();
    Task<List<Dream>> GetDreamsByCulture(string culture);
}
