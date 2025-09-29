using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using BLL.Manager.Interfaces;
using DAL.Interfaces;
using BLL.DTOs;
using Microsoft.AspNetCore.Authorization;
using RetouchAgency.Authorization;
using System.Security.Claims;

namespace RetouchAgency.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventBookingController(IEventBookingManager eventBookingManager) : ControllerBase
{
    private readonly IEventBookingManager _eventBookingManager = eventBookingManager;

    [HttpGet]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> GetAll()
    {
        var bookings = await _eventBookingManager.GetAllEventBookingsAsync();
        return Ok(bookings);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var booking = await _eventBookingManager.GetEventBookingByIdAsync(id);
        if (booking == null)
            return NotFound();
        return Ok(booking);
    }

    [HttpGet("user/{id}")]
    [AdminOrOwner]
    public async Task<IActionResult> GetBookingsByUserIdAsync(int id)
    {
        var booking = await _eventBookingManager.GetBookingsByUserIdAsync(id);
        if (booking == null)
            return NotFound();
        return Ok(booking);
    }

    [HttpGet("event/{id}")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> GetBookingsByEventIdAsync(int id)
    {
        var booking = await _eventBookingManager.GetBookingsByEventIdAsync(id);
        if (booking == null)
            return NotFound();
        return Ok(booking);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Book([FromBody] EventBookingDTO booking)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole(UserRole.Admin);
        if (!isAdmin && (userIdClaim == null || userIdClaim != booking.UserId.ToString()))
            return Forbid();
    
        try
        {
            await _eventBookingManager.BookEventAsync(booking);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        return Created();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _eventBookingManager.CancelBookingAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        return NoContent();
    }
}
