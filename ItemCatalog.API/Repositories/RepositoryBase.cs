using ItemCatalog.API.Models.Entities;
using ItemCatalog.API.Repositories.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ItemCatalog.API.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : IEntity, new()
{
    private readonly IConfiguration _configuration;
    private readonly string databaseName;
    private string CollectionName { get; init; } = typeof(T).Name.ToLower() + "s";
    protected readonly IMongoCollection<T> _collection;
    protected readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

    protected RepositoryBase(IMongoClient mongoClient, IConfiguration configuration)
    {
        _configuration = configuration;

        databaseName = _configuration.GetSection("MongoDbSettings:DatabaseName").Value;
        
        IMongoDatabase database = mongoClient.GetDatabase(databaseName);
        
        _collection = database.GetCollection<T>(CollectionName);
    }

    public async Task CreateAsync(T t)
    {
        await _collection.InsertOneAsync(t);
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = _filterBuilder.Eq(t => t.Id, id);
        await _collection.DeleteOneAsync(filter);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task UpdateAsync(T t)
    {
        var filter =_filterBuilder.Eq(existingT => existingT.Id, t.Id);
        await _collection.ReplaceOneAsync(filter, t);
    }
}