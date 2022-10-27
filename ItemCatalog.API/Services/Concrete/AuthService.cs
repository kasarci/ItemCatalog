using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ItemCatalog.API.Extensions;
using ItemCatalog.API.Models;
using ItemCatalog.API.Models.Dtos.UserDtos.Login;
using ItemCatalog.API.Models.Entities;
using ItemCatalog.API.Repositories.Abstract;
using ItemCatalog.API.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ItemCatalog.API.Services.Concrete;

public class AuthService : IAuthService
{
    private readonly IRefreshTokenRepository _refreshTokenRepo;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(IRefreshTokenRepository refreshTokenRepo, TokenValidationParameters tokenValidationParameters, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _refreshTokenRepo = refreshTokenRepo;
        _tokenValidationParameters = tokenValidationParameters;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        try
        {
            _tokenValidationParameters.ValidateLifetime = false; //for testing
            var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out SecurityToken? validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                if(result is false)
                {
                    return null;
                }
            }
            var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

            if(expiryDate < DateTime.UtcNow)
            {
                return new AuthResult().AddErrors("Expired token.");
            }

            var storedToken = await _refreshTokenRepo.GetAsync(tokenRequest.RefreshToken);
            if(storedToken is null)
            {
                return new AuthResult().AddErrors("Invalid token.");
            }

            if(storedToken.IsUsed)
            {
                return new AuthResult().AddErrors("Invalid token.");
            }

            if(storedToken.IsRevoked)
            {
                return new AuthResult().AddErrors("Invalid token.");
            }

            var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);

            if (storedToken.JwtId != jti.Value)
            {
                return new AuthResult().AddErrors("Invalid token.");
            }

            if(storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return new AuthResult().AddErrors("Expired token.");
            }

            storedToken.IsUsed = true;
            await _refreshTokenRepo.UpdateAsync(storedToken);

            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId.ToString());

            return await GenerateAuthResult(dbUser);
        }
        catch (System.Exception)
        {
            return new AuthResult().AddErrors("Server error");
        }
    }

    public async Task<AuthResult> GenerateAuthResult(ApplicationUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        byte[]? key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

        var authClaims = new ClaimsIdentity(new [] 
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("username", user.UserName),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
        });

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var userRole in userRoles)
        {
            authClaims.AddClaim(new Claim(ClaimTypes.Role, userRole));
        }

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = authClaims,
            Expires = DateTime.Now.Add((TimeSpan.Parse(_configuration.GetSection("JwtConfig:ExpiryTimeFrame").Value))),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var tokenString = jwtTokenHandler.WriteToken(token);

        var refreshToken = new RefreshToken()
        {
            JwtId = token.Id,
            Token = RandomStringGeneration(23),
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddMonths(6),
            IsRevoked = false,
            IsUsed= false,
            UserId = user.Id,
            Username = user.UserName
        };

        await _refreshTokenRepo.CreateAsync(refreshToken);

        return new AuthResult().AddTokens(tokenString, refreshToken.Token);
    }
    private static DateTime UnixTimeStampToDateTime(long utcExpiryDate)
    {
        var dateTime = new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc);
        return dateTime.AddSeconds(utcExpiryDate).ToUniversalTime();
    }

    private static string RandomStringGeneration(int length)
    {
        var random = new Random();
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_()";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(chars.Length)]).ToArray());
    }
}