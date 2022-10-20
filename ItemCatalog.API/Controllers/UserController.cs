using AutoMapper;
using ItemCatalog.API.Dtos;
using ItemCatalog.API.Dtos.UserDtos.Create;
using ItemCatalog.API.Dtos.UserDtos.Remove;
using ItemCatalog.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ItemCatalog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IActionResult> Create(CreateUserRequestDto user)
    {
        if(ModelState.IsValid)
        {
            ApplicationUser appUser = _mapper.Map<ApplicationUser>(user);

            var result = await _userManager.CreateAsync(appUser, user.Password);
            if (result.Succeeded)
            {
                return Ok(_mapper.Map<CreateUserResponseDto>(user));
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
            }
        }
        return BadRequest(ModelState);
    }

    public async Task<IActionResult> Remove(RemoveUserRequestDto user)
    {
        if (ModelState.IsValid)
        {
            var appUser = await _userManager.FindByEmailAsync(user.Email);
            if (appUser is null)
            {
                return NotFound();
            }

            IdentityResult result = await _userManager.DeleteAsync(appUser);
            if (result.Succeeded)
            {
                return Ok(new { Result = "User has succesfully deleted."});
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
            }
        }
        return BadRequest(ModelState);
    }
}