using BLL.DTOs;
using BLL.Manager.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserRequestsController : ControllerBase
    {
        private readonly IUserRequestsManager _userRequestManager;

        public UserRequestsController(IUserRequestsManager userRequestManager)
        {
            _userRequestManager = userRequestManager;
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _userRequestManager.GetRequestsAsync();
            return Ok(requests);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] CreateUserRequestDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                    return Unauthorized("User ID not found in token.");

                int userId = int.Parse(userIdClaim);

                var createdRequest = await _userRequestManager.CreateRequestAsync(userId, dto);
                return Ok(createdRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPut("{id}/status")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateRequestStatus(int id, [FromBody] UpdateRequestStatusDto dto)
        {
            var result = await _userRequestManager.UpdateRequestStatusAsync(id, dto);
            if (!result)
                return NotFound($"Request with ID {id} not found.");

            return Ok("Status updated successfully.");
        }

        
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteRequest([FromRoute]int id)
        {
            var result = await _userRequestManager.DeleteRequestAsync(id);
            if (!result)
                return NotFound($"Request with ID {id} not found.");

            return Ok("Request deleted successfully.");
        }
    }
}
