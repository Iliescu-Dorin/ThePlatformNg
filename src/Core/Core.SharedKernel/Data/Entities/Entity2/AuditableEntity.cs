namespace Core.SharedKernel.Data.Entities.Entity2;

public abstract class AuditableEntity : BaseEntity
{
    public virtual string CreatedBy { get; set; }
    public virtual DateTime Created { get; set; }
    public virtual string LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; }
}
