using DreamDomain.Entities;

namespace DreamApplication.Interfaces;
public interface IDreamService
{
    void CreateDream(Dream dream);
    Dream GetDreamById(Guid id);
    List<Dream> GetAllDreams();
    List<Dream> GetDreamsByCulture(string culture);
}