using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using BLL.Manager.Interfaces;
using DAL.Interfaces;
using BLL.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace RetouchAgency.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventBookingController(IEventBookingManager eventBookingManager) : ControllerBase
{
    private readonly IEventBookingManager _eventBookingManager = eventBookingManager;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var bookings = await _eventBookingManager.GetAllEventBookingsAsync();
        return Ok(bookings);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var booking = await _eventBookingManager.GetEventBookingByIdAsync(id);
        if (booking == null)
            return NotFound();
        return Ok(booking);
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetBookingsByUserIdAsync(int id)
    {
        var booking = await _eventBookingManager.GetBookingsByUserIdAsync(id);
        if (booking == null)
            return NotFound();
        return Ok(booking);
    }

    [HttpGet("event/{id}")]
    public async Task<IActionResult> GetBookingsByEventIdAsync(int id)
    {
        var booking = await _eventBookingManager.GetEventBookingByIdAsync(id);
        if (booking == null)
            return NotFound();
        return Ok(booking);
    }

    [HttpPost]
    [Authorize(Roles = UserRole.Applicant)]
    public async Task<IActionResult> Book([FromBody] EventBookingDTO booking)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            await _eventBookingManager.BookEventAsync(booking);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        return CreatedAtAction(nameof(GetByIdAsync), new { id = booking.EventId }, booking);
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
