using ItemCatalog.API.Models.Entities;

namespace ItemCatalog.API.Repositories.Abstract;

public interface IRepositoryBase<T> where T : IEntity, new()
{
    Task<IEnumerable<T>> GetAllAsync();
    Task CreateAsync(T t);
    Task UpdateAsync(T t);
    Task DeleteAsync(Guid id);
}
