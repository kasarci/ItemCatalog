using ItemCatalog.API.Models.Entities;
using ItemCatalog.API.Repositories.Abstract;
using MongoDB.Driver;

namespace ItemCatalog.API.Repositories;

public class MongoDbItemsRepository : RepositoryBase<Item>, IItemsRepository
{
    public MongoDbItemsRepository(IMongoClient mongoClient, IConfiguration configuration) : base(mongoClient, configuration) { }
}