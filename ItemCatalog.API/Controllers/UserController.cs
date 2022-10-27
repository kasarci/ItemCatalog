using AutoMapper;
using ItemCatalog.API.Models.Dtos.UserDtos.Create;
using ItemCatalog.API.Models.Dtos.UserDtos.Remove;
using ItemCatalog.API.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ItemCatalog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IMapper _mapper;

    public UserController(UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _roleManager = roleManager;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(CreateUserRequestDto user)
    {
        if(ModelState.IsValid)
        {
            ApplicationUser appUser = _mapper.Map<ApplicationUser>(user);
            //Adding admin role to users for test purposes.
            var role = await _roleManager.FindByNameAsync("Admin");
            appUser.AddRole(role.Id);

            var result = await _userManager.CreateAsync(appUser, user.Password);
            

            if (result.Succeeded)
            {
                return Ok(_mapper.Map<CreateUserResponseDto>(appUser));
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
            }
        }
        return BadRequest(ModelState);
    }

    [HttpPost]
    [Route("delete")]
    [Authorize("RequireAdminRole")]
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