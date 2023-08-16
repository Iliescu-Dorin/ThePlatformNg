namespace Core.SharedKernel.Interfaces;
public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAll();
    Task<T> Get(object id);
    Task Add(T entity);
    Task Update(T entity);
    Task Delete(object id);
    Task<int> Count();
}
