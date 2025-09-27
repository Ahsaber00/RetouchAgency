using BLL.DTOs;
using BLL.Manager.Interfaces;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiRetouchAgency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationManager _applicationManager;
        public ApplicationController(IApplicationManager applicationManager)
        {
            _applicationManager = applicationManager;
        }


        [HttpGet]
        public async Task<IActionResult> GetApplications([FromQuery] string? status = null)
        {
            var applications= await _applicationManager.GetAllApplicationsAsync(status);
            return Ok(applications);
        }

        [HttpGet("opportunity/{opportunityId:int}")]
        public async Task<IActionResult> GetApplicationsPerOpportunity([FromRoute] int opportunityId)
        {
            var applications = await _applicationManager.GetApplicationsByOpportunityAsync(opportunityId);
            return Ok(applications);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetApplicationsPerUser([FromRoute] int userId)
        {
            var applications = await _applicationManager.GetApplicationsByUserAsync(userId);
            return Ok(applications);
        }

        [HttpGet("{applicationId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int applicationId)
        {
            var application=await _applicationManager.GetApplicationByIdAsync(applicationId);
            if(application==null)
            {
                return NotFound();
            }
            return Ok(application);
        }


        [HttpPost]
        public async Task<IActionResult> CreateApplication(CreateApplicationDto dto)
        {
            try
            {
                var newApplication = await _applicationManager.CreateApplicationAsync(dto);
                return Ok(newApplication);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut]
        public async Task<IActionResult> UpdateApplication(int id,UpdateApplicationDto dto)
        {
            try
            {
                var application = await _applicationManager.UpdateApplicationAsync(id, dto);
                return Ok(application);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteApplication([FromRoute]int id)
        {
            var result= await _applicationManager.DeleteApplicationAsync(id);
            if(result is true)
            {
                return Ok("Deleted Successfully!");
            }
            return NotFound();
        }

       
        

    }
}
