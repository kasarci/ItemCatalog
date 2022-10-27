using ItemCatalog.API.Models;

namespace ItemCatalog.API.Repositories.Abstract;

public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken> 
{
    Task<RefreshToken> GetAsync(string id);
    Task DeleteByUserName(string? username);
}
