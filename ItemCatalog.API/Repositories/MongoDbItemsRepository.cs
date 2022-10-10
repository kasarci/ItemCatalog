using System.ComponentModel;
using System.Runtime.CompilerServices;
using ItemCatalog.API.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ItemCatalog.API.Repositories;

public class MongoDbItemsRepository : IItemsRepository
{
    private const string databaseName = "catalog";
    private const string collectionName = "items";
    private readonly IMongoCollection<Item> _itemsCollection;
    private readonly FilterDefinitionBuilder<Item> _filterBuilder = Builders<Item>.Filter;

    public MongoDbItemsRepository(IMongoClient mongoClient)
    {
        IMongoDatabase database = mongoClient.GetDatabase(databaseName);
        _itemsCollection = database.GetCollection<Item>(collectionName);
    }

    public async Task CreateItemAsync(Item item)
    {
        await _itemsCollection.InsertOneAsync(item);
    }

    public async Task DeleteItemAsync(Guid id)
    {
        var filter = _filterBuilder.Eq(item => item.Id, id);
        await _itemsCollection.DeleteOneAsync(filter);
    }

    public async Task<Item> GetItemAsync(Guid id)
    {
        var filter = _filterBuilder.Eq(item => item.Id, id);
        return await  _itemsCollection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Item>> GetItemsAsync()
    {
        return await _itemsCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task UpdateItemAsync(Item item)
    {
        var filter =_filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
        await _itemsCollection.ReplaceOneAsync(filter, item);
    }
}