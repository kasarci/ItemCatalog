using ItemCatalog.API.Models.Dtos.RoleDtos;
using ItemCatalog.API.Models.Entities;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ItemCatalog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class RoleController : ControllerBase
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RoleController(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IActionResult> CreateRole(RoleDto roleDto)
    {
        if(ModelState.IsValid)
        {
            var result = await _roleManager.CreateAsync(new ApplicationRole(){Name = roleDto.Name});
            if(result.Succeeded)
            {
                return Ok(new {Result= "Role has successfully created."});
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
            }
        }
        return BadRequest(ModelState);
    }

    public async Task<IActionResult> DeleteRole(RoleDto roleDto)
    {
        if(ModelState.IsValid)
        {
            var role = await _roleManager.FindByNameAsync(roleDto.Name);

            if(role is null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);

            if(result.Succeeded)
            {
                return Ok(new { Result = "Role has succesfully deleted."});
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