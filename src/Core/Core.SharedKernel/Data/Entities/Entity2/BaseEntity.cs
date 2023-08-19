namespace Core.SharedKernel.Data.Entities.Entity2;

public abstract class BaseEntity
{
    public virtual int Id { get; set; }
    public virtual DateTime Created { get; set; }
}