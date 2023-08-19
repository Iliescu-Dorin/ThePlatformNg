namespace Core.SharedKernel.Data.Entities.Entity3.Interfaces;

public interface ISoftDeletable
{
    public string? DeletedBy { get; }

    public DateTime? DeletedAt { get; }
}
