using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using ItemCatalog.API.Extensions;
using ItemCatalog.API.Models;
using ItemCatalog.API.Models.Dtos.UserDtos.Login;
using ItemCatalog.API.Models.Entities;
using ItemCatalog.API.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.IdentityModel.Tokens;

namespace ItemCatalog.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IRefreshTokenRepository _refreshTokenRepo;
    private readonly TokenValidationParameters _tokenValidationParameters;


    public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMapper mapper, IRefreshTokenRepository refreshTokenRepo, TokenValidationParameters tokenValidationParameters)
    {
        _userManager = userManager;
        _configuration = configuration;
        _mapper = mapper;
        _refreshTokenRepo = refreshTokenRepo;
        _tokenValidationParameters = tokenValidationParameters;
    }

    [HttpPost]
    [Route("[controller]")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequestDto loginRequestDto)
    {
        if (ModelState.IsValid)
        {
            var existingUser = await _userManager.FindByEmailAsync(loginRequestDto.Email);
            if (existingUser is null)
            {
                return BadRequest(new AuthResult().AddError("Invalid user."));
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(existingUser,loginRequestDto.Password);
            if (!isPasswordCorrect)
            {
                return BadRequest(new AuthResult().AddError("Invalid password."));
            }

            string jwtToken = GenerateJwtToken(existingUser);

            return Ok(new AuthResult().AddToken(jwtToken));
        }
        return BadRequest(new AuthResult().AddError("Invalid payload."));
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        byte[]? key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new [] 
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
            }),
            Expires = DateTime.Now.Add((TimeSpan.Parse(_configuration.GetSection("JwtConfig:ExpiryTimeFrame").Value))),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        return jwtTokenHandler.WriteToken(token);
    }
}