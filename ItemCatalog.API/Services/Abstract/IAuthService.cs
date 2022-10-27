using ItemCatalog.API.Models;
using ItemCatalog.API.Models.Dtos.UserDtos.Login;
using ItemCatalog.API.Models.Entities;

namespace ItemCatalog.API.Services.Abstract;

public interface IAuthService
{
    Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest);
    Task<AuthResult> GenerateAuthResult(ApplicationUser user);
}