using DreamData.Domain.Entities;

namespace DreamData.Application.Services.Interfaces;
public interface IDreamDataService
{
    Task AddDream(Dream dream);
    Task<Dream> GetDreamById(Guid id);
    Task<List<Dream>> GetAllDreams();
    Task<List<Dream>> GetDreamsByCulture(string culture);
}
