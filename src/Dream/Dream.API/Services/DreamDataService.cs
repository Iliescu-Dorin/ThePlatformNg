using DreamData.Application.Services.Interfaces;
using DreamData.Domain.Entities;
using DreamData.Infrastructure.Interfaces;

namespace DreamApplication.Services;

public class DreamDataService : IDreamDataService
{
    private readonly IDreamDataRepository _dreamRepository;

    public DreamDataService(IDreamDataRepository dreamRepository)
    {
        _dreamRepository = dreamRepository;
    }

    public Task AddDream(Dream dream) =>
        _dreamRepository.AddDream(dream);


    public Task<Dream> GetDreamById(Guid id) =>
        _dreamRepository.GetDreamById(id);

    public Task<List<Dream>> GetAllDreams() =>
        _dreamRepository.GetAllDreams();

    public Task<List<Dream>> GetDreamsByCulture(string culture) =>
         _dreamRepository.GetDreamsByCulture(culture);
}
