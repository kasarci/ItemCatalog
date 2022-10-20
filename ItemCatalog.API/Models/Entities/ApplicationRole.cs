using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace ItemCatalog.API.Models.Entities;

[CollectionName("Roles")]
public class ApplicationRole : MongoIdentityRole<Guid> { }