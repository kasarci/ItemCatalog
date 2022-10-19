using ItemCatalog.API.Entities;
using ItemCatalog.API.Repositories.Abstract;
using MongoDB.Driver;

namespace ItemCatalog.API.Repositories;

public class MongoDbItemsRepository : RepositoryBase<Item>, IItemsRepository
{
    public MongoDbItemsRepository(IMongoClient mongoClient) : base(mongoClient) { }
}