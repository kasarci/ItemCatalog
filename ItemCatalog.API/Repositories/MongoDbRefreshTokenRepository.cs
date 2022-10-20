using ItemCatalog.API.Models;
using ItemCatalog.API.Repositories.Abstract;
using MongoDB.Driver;

namespace ItemCatalog.API.Repositories;

public class MongoDbRefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
{
    public MongoDbRefreshTokenRepository(IMongoClient mongoClient, IConfiguration configuration) : base(mongoClient, configuration) { }
}