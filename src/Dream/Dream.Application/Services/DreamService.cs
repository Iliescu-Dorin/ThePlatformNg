using DreamApplication.Interfaces;
using DreamDomain.Entities;
using DreamInfrastructure.Interfaces;

namespace DreamApplication.Services;

public class DreamService : IDreamService
{
    private readonly IDreamRepository _dreamRepository;

    public DreamService(IDreamRepository dreamRepository)
    {
        _dreamRepository = dreamRepository;
    }

    public void CreateDream(Dream dream) =>
        _dreamRepository.AddDream(dream);


    public Dream GetDreamById(Guid id) =>
        _dreamRepository.GetDreamById(id);

    public List<Dream> GetAllDreams() =>
        _dreamRepository.GetAllDreams();

    public List<Dream> GetDreamsByCulture(string culture) =>
        _dreamRepository.GetDreamsByCulture(culture);
}
