using ItemCatalog.API.Models.Entities;
using ItemCatalog.API.Repositories.Abstract;
using MongoDB.Driver;

namespace ItemCatalog.API.Repositories;

public class MongoDbItemsRepository : RepositoryBase<Item>, IItemsRepository
{
    public MongoDbItemsRepository(IMongoClient mongoClient, IConfiguration configuration) : base(mongoClient, configuration) { }

    public async Task<Item> GetAsync(Guid id)
    {
        var filter = _filterBuilder.Eq(t => t.Id, id);
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }
}