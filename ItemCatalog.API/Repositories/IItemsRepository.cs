using ItemCatalog.API.Entities;

namespace ItemCatalog.API.Repositories;

public interface IItemsRepository
{
    Item GetItem(Guid id);
    IEnumerable<Item> GetItems();
}
