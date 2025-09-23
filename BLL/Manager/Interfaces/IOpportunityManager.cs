using BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Manager.Interfaces
{
    public interface IOpportunityManager
    {
        Task<OpportunityDto> CreateOpportunityAsync(CreateOpportunityDto dto);
        Task<OpportunityDto> UpdateOpportunityAsync(int id, UpdateOpportunityDto dto);
        Task<bool> DeleteOpportunityAsync(int id);
        Task<OpportunityDto> GetOpportunityByIdAsync(int id);
        Task<IEnumerable<OpportunityDto>> GetAllOpportunitiesAsync(string opportunityType=null);
    }
}
