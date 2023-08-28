using Core.Services.Interfaces;
using DreamData.Domain.Entities;

namespace DreamData.Infrastructure.Interfaces;
public interface IDreamRepository : IRepository<Dream>
{
    Task<List<Dream>> GetDreamsByCulture(string culture);
}
