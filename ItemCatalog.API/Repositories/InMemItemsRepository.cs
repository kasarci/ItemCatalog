using ItemCatalog.API.Entities;

namespace ItemCatalog.API.Repositories;

public class InMemItemsRepository : IItemsRepository
{
    private readonly List<Item> items = new()
    {
        new Item{Id= Guid.NewGuid(), Name= "Potion", Price=9, CreatedDate= DateTimeOffset.UtcNow},
        new Item{Id= Guid.NewGuid(), Name= "Iron Sword", Price=20, CreatedDate= DateTimeOffset.UtcNow},
        new Item{Id= Guid.NewGuid(), Name= "Bronze Shield", Price=9, CreatedDate= DateTimeOffset.UtcNow}
    };

    public IEnumerable<Item> GetItems()
    {
        return items;
    }

    public Item GetItem(Guid id)
    {
        return items.Where(x => x.Id == id).SingleOrDefault();
    }
}
