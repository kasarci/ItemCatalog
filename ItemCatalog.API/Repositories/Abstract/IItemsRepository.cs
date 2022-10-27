using ItemCatalog.API.Models.Entities;

namespace ItemCatalog.API.Repositories.Abstract;

public interface IItemsRepository : IRepositoryBase<Item> 
{
    Task<Item> GetAsync(Guid id);
}
