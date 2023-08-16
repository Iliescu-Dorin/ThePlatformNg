using Core.SharedKernel.Entities;

namespace Core.SharedKernel.Data.Aggregators;
public class AggregateRoot<TId> : Entity<TId> where TId : class
{
    protected AggregateRoot(TId id) : base(id)
    {
    }
}
