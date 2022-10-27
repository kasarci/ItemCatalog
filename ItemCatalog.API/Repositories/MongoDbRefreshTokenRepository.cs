using ItemCatalog.API.Models;
using ItemCatalog.API.Repositories.Abstract;
using MongoDB.Driver;

namespace ItemCatalog.API.Repositories;

public class MongoDbRefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
{
    public MongoDbRefreshTokenRepository(IMongoClient mongoClient, IConfiguration configuration) : base(mongoClient, configuration) { }

    public async Task<RefreshToken> GetAsync(string id)
    {
        var filter = _filterBuilder.Eq(t => t.Token, id);
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task DeleteByUserName(string? username)
    {
        var filter = _filterBuilder.Eq(t => t.Username, username);
        await _collection.DeleteOneAsync(filter);
    }
}