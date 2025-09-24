using BLL.DTOs;
using BLL.Manager;
using BLL.Manager.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiRetouchAgency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpportunitiesController : ControllerBase
    {
        private readonly IOpportunityManager _opportunityManager;
        public OpportunitiesController(IOpportunityManager opportunityManager)
        {
            _opportunityManager = opportunityManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetOpportunities([FromQuery]string? opportunityType=null)
        {
            var opportunities = await _opportunityManager.GetAllOpportunitiesAsync(opportunityType);
            return Ok(opportunities);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var opportunity = await _opportunityManager.GetOpportunityByIdAsync(id);
            if (opportunity == null)
            {
                return NotFound();
            }
            return Ok(opportunity);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateOpportunityDto dto)
        {
            try
            {
                var opportunity = await _opportunityManager.CreateOpportunityAsync(dto);
                return Ok(opportunity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute]int id,[FromBody]UpdateOpportunityDto dto)
        {
            try
            {
                var opportunity = await _opportunityManager.UpdateOpportunityAsync(id,dto);
                return Ok(opportunity);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var result=await _opportunityManager.DeleteOpportunityAsync(id);
            if(result==true)
            {
                return Ok("Deleted Successfully!");
            }
            return NotFound();
        }



    }
}
