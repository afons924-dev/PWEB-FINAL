using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMEDIA.Shared.Data;

namespace MyMEDIA.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
    {
        return await _userManager.Users.ToListAsync();
    }

    // PUT: api/Users/{id}/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(string id, [FromBody] string status)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        user.Status = status;
        await _userManager.UpdateAsync(user);
        return NoContent();
    }

    // POST: api/Users/{id}/role
    [HttpPost("{id}/role")]
    public async Task<IActionResult> UpdateRole(string id, [FromBody] string roleName)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        if (!await _roleManager.RoleExistsAsync(roleName))
             await _roleManager.CreateAsync(new IdentityRole(roleName));

        var userRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, userRoles);
        await _userManager.AddToRoleAsync(user, roleName);

        // Update local property too for convenience
        user.UserType = roleName;
        await _userManager.UpdateAsync(user);

        return NoContent();
    }
}
