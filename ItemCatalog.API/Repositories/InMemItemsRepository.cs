using ItemCatalog.API.Models.Entities;

namespace ItemCatalog.API.Repositories;

public class InMemItemsRepository //: IItemsRepository
{
    private readonly List<Item> items = new()
    {
        new Item{Id= Guid.NewGuid(), Name= "Potion", Price=9, CreatedDate= DateTimeOffset.UtcNow},
        new Item{Id= Guid.NewGuid(), Name= "Iron Sword", Price=20, CreatedDate= DateTimeOffset.UtcNow},
        new Item{Id= Guid.NewGuid(), Name= "Bronze Shield", Price=9, CreatedDate= DateTimeOffset.UtcNow}
    };

    public IEnumerable<Item> GetItemsAsync()
    {
        return items;
    }

    public Item GetItemAsync(Guid id)
    {
        return items.Where(x => x.Id == id).SingleOrDefault();
    }

    public void CreateItemAsync(Item item)
    {
        items.Add(item);
    }

    public void UpdateItemAsync(Item item)
    {
        var index = items.FindIndex(x => x.Id == item.Id);
        items[index] = item;
    }

    public void DeleteItemAsync(Guid id)
    {
        var index = items.FindIndex(x => x.Id == id );
        items.RemoveAt(index);
    }
}
