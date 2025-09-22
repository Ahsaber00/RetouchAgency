using DAL.Repositories;
using DAL.Models;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace RetouchAgency.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        if (!ModelState.IsValid || user == null)
            return BadRequest(ModelState);
        await _userRepository.AddAsync(user);

        return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
    {
        // if (user == null || id != user.UserId || !ModelState.IsValid)
        //     return BadRequest(ModelState);
        if (user == null)
            return BadRequest("User data is null.");
        if (!ModelState.IsValid)
            return BadRequest("another error");
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
            return NotFound();
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.PasswordHash = user.PasswordHash;
        existingUser.GoogleId = user.GoogleId;
        existingUser.AuthMethod = user.AuthMethod;
        existingUser.Role = user.Role;
        _userRepository.Update(existingUser);
        return NoContent(); // 204 No Content
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
            return NotFound();
        await _userRepository.DeleteAsync(id);
        return NoContent(); // 204 No Content
    }
}