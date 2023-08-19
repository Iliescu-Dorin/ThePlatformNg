using Core.SharedKernel.Data.Entities.Entity3.Interfaces;

namespace Core.SharedKernel.Data.Entities.Entity3;

public abstract class MyEntity : IEntity, ISoftDeletable, IAudited
{
    public int Id { get; private set; }

    public string CreatedBy { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }

    public string? LastModifiedBy { get; private set; }

    public DateTime? LastModifiedAt { get; private set; }

    public string? DeletedBy { get; private set; }

    public DateTime? DeletedAt { get; private set; }
}
