using BLL.DTOs;
using BLL.Manager.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetouchAgency.Authorization;
using System.Security.Claims;

namespace RetouchAgency.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController(IEventManager eventManager) : ControllerBase
    {
        private readonly IEventManager _eventManager = eventManager;

        /// <summary>
        /// Get all events (Public endpoint)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            try
            {
                var events = await _eventManager.GetAllEventsAsync();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get a specific event by ID (Public endpoint)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            try
            {
                var eventDto = await _eventManager.GetEventByIdAsync(id);
                if (eventDto == null)
                    return NotFound("Event not found.");

                return Ok(eventDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Create a new event (Authenticated users only)
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateDTO eventCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
                    return Unauthorized("Invalid token.");

                var eventId = await _eventManager.CreateEventAsync(eventCreateDto, userId);
                return CreatedAtAction(nameof(GetEventById), new { id = eventId }, new { id = eventId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an event (Admin or event owner only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventCreateDTO eventUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
                    return Unauthorized("Invalid token.");

                await _eventManager.UpdateEventAsync(id, eventUpdateDto, userId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete an event (Admin or event owner only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
                    return Unauthorized("Invalid token.");

                await _eventManager.DeleteEventAsync(id, userId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get all events created by a specific user (Public endpoint)
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetEventsByUser(int userId)
        {
            try
            {
                var events = await _eventManager.GetEventsByUserAsync(userId);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get all events created by the current authenticated user
        /// </summary>
        [HttpGet("my-events")]
        [Authorize]
        public async Task<IActionResult> GetMyEvents()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
                    return Unauthorized("Invalid token.");

                var events = await _eventManager.GetEventsByUserAsync(userId);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}