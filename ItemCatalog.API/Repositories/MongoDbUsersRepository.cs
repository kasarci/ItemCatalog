using ItemCatalog.API.Entities;
using ItemCatalog.API.Repositories.Abstract;
using MongoDB.Driver;

namespace ItemCatalog.API.Repositories;

public class MongoDbUsersRepository : RepositoryBase<User>, IUsersRepository
{
    public MongoDbUsersRepository(IMongoClient mongoClient) : base(mongoClient) { }
}