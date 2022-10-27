using ItemCatalog.API.Extensions;
using ItemCatalog.API.Models;
using ItemCatalog.API.Models.Dtos.UserDtos.Login;
using ItemCatalog.API.Models.Entities;
using ItemCatalog.API.Repositories.Abstract;
using ItemCatalog.API.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ItemCatalog.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AuthController> _logger;
    private readonly IRefreshTokenRepository _refreshTokenRepo;
    private readonly IAuthService _authService;


    public AuthController(UserManager<ApplicationUser> userManager,  IRefreshTokenRepository refreshTokenRepo,  ILogger<AuthController> logger, IAuthService authService)
    {
        _userManager = userManager;
        _refreshTokenRepo = refreshTokenRepo;
        _logger = logger;
        _authService = authService;
    }

    [HttpPost]
    [Route("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUserRequestDto loginRequestDto)
    {
        if (ModelState.IsValid)
        {
            var existingUser = await _userManager.FindByEmailAsync(loginRequestDto.Email);
            if (existingUser is null)
            {
                return BadRequest(new AuthResult().AddErrors("Invalid user."));
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(existingUser,loginRequestDto.Password);
            if (!isPasswordCorrect)
            {
                return BadRequest(new AuthResult().AddErrors("Invalid password."));
            }

            var roles = await _userManager.GetRolesAsync(existingUser);
            _logger.LogInformation($"[{loginRequestDto.Email}] logged in the system with the {String.Join(",", roles)} roles.");
            return Ok(await _authService.GenerateAuthResult(existingUser));
        }
        return BadRequest(new AuthResult().AddErrors("Invalid payload."));
    }

    [HttpPost]
    [Route("Logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var username = User.Identity?.Name;
        await _refreshTokenRepo.DeleteByUserName(username);
        _logger.LogInformation($"User [{username}] logged out the system.");
        return Ok();
    }

    [HttpPost]
    [Route("RefreshToken")]
    [Authorize]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
    {
        if(ModelState.IsValid)
        {
            var result = await _authService.VerifyAndGenerateToken(tokenRequest);

            if (result is null)
            {
                return BadRequest(new AuthResult().AddErrors("Invalid tokens."));
            }

            return Ok(result);
        }
        return BadRequest(new AuthResult().AddErrors("Invalid parameters."));
    }
}