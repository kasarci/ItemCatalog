using ItemCatalog.API.Entities;

namespace ItemCatalog.API.Repositories.Abstract;

public interface IRepositoryBase<T> where T : IEntity, new()
{
    Task<T> GetAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task CreateAsync(T t);
    Task UpdateAsync(T t);
    Task DeleteAsync(Guid id);
}
