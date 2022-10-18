using System.Xml.Serialization;
using ItemCatalog.API.Entities;
using Microsoft.AspNetCore.Components.Web;

namespace ItemCatalog.API.Repositories;

public interface IRepositoryBase<T> where T : IEntity, new()
{
    Task<T> GetAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task CreateAsync(T t);
    Task UpdateAsync(T t);
    Task DeleteAsync(Guid id);
}
