using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace ItemCatalog.API.Entities;

[CollectionName("Roles")]
public class ApplicationRole : MongoIdentityRole<Guid> { }