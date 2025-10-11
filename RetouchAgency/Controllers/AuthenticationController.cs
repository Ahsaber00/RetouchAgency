namespace ApiRetouchAgency.Controllers;
using Microsoft.AspNetCore.Mvc;
using DAL.Interfaces;
using BLL.Manager.Interfaces;
using BLL.DTOs;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthenticationController(IUserManager userManager) : ControllerBase
{
    readonly IUserManager _userManager = userManager;

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] UserSignUpDTO user)
    {
        if (!ModelState.IsValid || user == null)
            return BadRequest(ModelState);
        try
        {
            UserDTO userDTO = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                
                Role = UserRole.Applicant, // Default role
                
            };
            await _userManager.CreateUserAsync(userDTO);
            return CreatedAtAction(nameof(SignUp), new { id = userDTO.Id }, new { id = userDTO.Id });
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO login)
    {
        if (!ModelState.IsValid || login == null)
            return BadRequest(ModelState);
        var token = await _userManager.LoginUserAsync(login);
        if (token == null)
            return Unauthorized("Invalid email or password.");
        return Ok(new { Token = token });
    }
}