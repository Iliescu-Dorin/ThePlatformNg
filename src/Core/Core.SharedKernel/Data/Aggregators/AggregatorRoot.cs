using Core.SharedKernel.Data.Entities.Entity1;

namespace Core.SharedKernel.Data.Aggregators;
public class AggregateRoot<TId> : Entity<TId> where TId : class
{
    protected AggregateRoot(TId id) : base(id)
    {
    }
}
