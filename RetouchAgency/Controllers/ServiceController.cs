using BLL.DTOs;
using BLL.Manager.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiRetouchAgency.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ServiceController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _serviceManager.GetAllServicesAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var service = await _serviceManager.GetServiceByIdAsync(id);
            if (service == null)
                return NotFound("Service not found.");
            
            return Ok(service);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetServicesByUser(int userId)
        {
            var services = await _serviceManager.GetServicesByUserAsync(userId);
            return Ok(services);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateService([FromBody] ServiceCreateDTO serviceCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Get the user ID from the authenticated user's claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                    return Unauthorized("Invalid token.");

                var serviceId = await _serviceManager.CreateServiceAsync(serviceCreateDto, userId);
                return CreatedAtAction(nameof(GetServiceById), new { id = serviceId }, new { id = serviceId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateService(int id, [FromBody] ServiceCreateDTO serviceUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Get the user ID from the authenticated user's claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var requestingUserId))
                    return Unauthorized("Invalid token.");

                await _serviceManager.UpdateServiceAsync(id, serviceUpdateDto, requestingUserId);
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
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteService(int id)
        {
            try
            {
                // Get the user ID from the authenticated user's claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var requestingUserId))
                    return Unauthorized("Invalid token.");

                await _serviceManager.DeleteServiceAsync(id, requestingUserId);
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
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("my-services")]
        [Authorize]
        public async Task<IActionResult> GetMyServices()
        {
            try
            {
                // Get the user ID from the authenticated user's claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                    return Unauthorized("Invalid token.");

                var services = await _serviceManager.GetServicesByUserAsync(userId);
                return Ok(services);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}