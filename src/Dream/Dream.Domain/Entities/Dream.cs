using Core.SharedKernel.Entities;

namespace DreamDomain.Entities;
public class Dream : Entity<Guid> {
  public Dream(Guid id) : base(id) {}

  public new Guid Id { get;
  set;
}
public Guid UserId { get; set; }
public string Title { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public DateTime Date { get; set; } = DateTime.UtcNow;
public List<string>? Symbols { get; set; } = new List<string>();
public List<Interpretation> Interpretations {
  get; set;
} = new List<Interpretation>();
}
