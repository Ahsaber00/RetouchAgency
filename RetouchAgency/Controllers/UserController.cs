using DAL.Repositories;
using DAL.Models;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BLL.Manager.Interfaces;
using BLL.DTOs;
using Microsoft.AspNetCore.Authorization;
namespace ApiRetouchAgency.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserManager userManager) : ControllerBase
{
    private readonly IUserManager _userManager = userManager;

    [HttpGet]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _userManager.GetAllUsersAsync());
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserById(int id)
    {
        var token = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        var userRoleClaim = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        if (userIdClaim == null)
            return Unauthorized("Invalid token.");
        if (userRoleClaim != UserRole.Admin && userIdClaim != id.ToString())
            return Forbid("You do not have permission to access this resource.");
        var user = await _userManager.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    [HttpPost]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> CreateUser([FromBody] UserDTO user)
    {
        if (!ModelState.IsValid || user == null)
            return BadRequest(ModelState);
        try
        {
            await _userManager.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new { id = user.Id });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message); // 409 Conflict
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message); // 400 Bad Request
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO user)
    {
        if (user == null)
            return BadRequest("User data is null.");
        if (!ModelState.IsValid)
            return BadRequest();
        var token = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        var userRoleClaim = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        if (userIdClaim == null)
            return Unauthorized("Invalid token.");
        if (userRoleClaim != UserRole.Admin && userIdClaim != id.ToString())
            return Forbid("You do not have permission to access this resource.");
        try
        {
            await _userManager.UpdateUserAsync(id, user);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        return NoContent(); // 204 No Content
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var token = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        var userRoleClaim = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        if (userIdClaim == null)
            return Unauthorized("Invalid token.");
        if (userRoleClaim != UserRole.Admin && userIdClaim != id.ToString())
            return Forbid("You do not have permission to access this resource.");
        try
        {
            await _userManager.DeleteUserAsync(id);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        return NoContent(); // 204 No Content
    }
}