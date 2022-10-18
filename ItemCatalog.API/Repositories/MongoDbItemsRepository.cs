using System.ComponentModel;
using System.Runtime.CompilerServices;
using ItemCatalog.API.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ItemCatalog.API.Repositories;

public class MongoDbItemsRepository : RepositoryBase<Item>, IItemsRepository
{
    public MongoDbItemsRepository(IMongoClient mongoClient) : base(mongoClient) { }
}