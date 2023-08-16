using Core.SharedKernel.Entities;

namespace DreamData.Domain.Entities;
// Or public class Dream : AuditableEntity  similar to https://github.dev/referbruv/ContainerNinja.CleanArchitecture/blob/9a725d3db2e287580a65319d5717c848fad32551/API/ContainerNinja.Core/ServiceExtensions.cs#L21#L65
public class Dream : Entity<Guid>
{
    public Dream(Guid id) : base(id)
    {
    }

    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public List<string>? Symbols { get; set; } = new List<string>();
    public List<Interpretation> Interpretations { get; set; } = new List<Interpretation>();
}
