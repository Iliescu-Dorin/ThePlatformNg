using DreamDomain.Entities;

namespace DreamInfrastructure.Interfaces;
public interface IDreamRepository {
  void AddDream(Dream dream);
  Dream GetDreamById(Guid id);
  List<Dream> GetAllDreams();
  List<Dream> GetDreamsByCulture(string culture);
}
