using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace ItemCatalog.API.Models.Entities;

[CollectionName("Users")]
public class ApplicationUser : MongoIdentityUser<Guid>, IEntity { }