using Core.SharedKernel.Data.Entities.Entity2;

namespace Core.SharedKernel.DTO;
public class DreamDTO : AuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Categories { get; set; }
}

