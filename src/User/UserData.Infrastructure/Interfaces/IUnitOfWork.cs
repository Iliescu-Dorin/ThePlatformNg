namespace UserData.Infrastructure.Interfaces;
public interface IUnitOfWork
{
    //IItemRepository Items { get; }
    IUserRepository Users { get; }
    Task CommitAsync();
}
