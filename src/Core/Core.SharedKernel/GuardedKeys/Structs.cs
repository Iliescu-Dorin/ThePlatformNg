namespace Core.SharedKernel.GuardedKeys;

public readonly struct DreamId
{
    public DreamId(Guid value) => Value = value;
    public Guid Value { get; }
}