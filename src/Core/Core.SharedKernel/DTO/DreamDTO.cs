using Core.SharedKernel.Entities;

namespace Core.SharedKernel.DTO;
public class DreamDTO : AuditableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Categories { get; set; }
    public string ColorCode { get; set; }
}

