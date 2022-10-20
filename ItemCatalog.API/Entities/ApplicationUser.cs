using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace ItemCatalog.API.Entities;

[CollectionName("Users")]
public class ApplicationUser : MongoIdentityUser<Guid>, IEntity { }