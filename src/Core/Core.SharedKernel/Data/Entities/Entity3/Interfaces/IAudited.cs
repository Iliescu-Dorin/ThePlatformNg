namespace Core.SharedKernel.Data.Entities.Entity3.Interfaces;

public interface IAudited
{
    string CreatedBy { get; }

    DateTime CreatedAt { get; }

    string? LastModifiedBy { get; }

    DateTime? LastModifiedAt { get; }
}
