namespace DreamData.Infrastructure.Interfaces;
public interface IUnitOfWork
{
    //IItemRepository Items { get; }
    IDreamRepository Dreams { get; }
    Task CommitAsync();
}
