using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Manager.Interfaces
{
    public interface IApplicationManager
    {
        // Create (User applies to an opportunity)
        Task<ApplicationDto> CreateApplicationAsync(CreateApplicationDto dto,int userId);

        Task<IEnumerable<ApplicationDto>> GetAllApplicationsAsync(string? status = null);

        // Get applications for a specific opportunity
        Task<IEnumerable<ApplicationDto>> GetApplicationsByOpportunityAsync(int opportunityId);

        // Get applications for a specific user
        Task<IEnumerable<ApplicationDto>> GetApplicationsByUserAsync(int userId);

        Task<ApplicationDto?> GetApplicationByIdAsync(int id);

        // Update application (admin can update status: Submitted, Reviewed, Hired)
        Task<ApplicationDto?> UpdateApplicationAsync(int id, UpdateApplicationDto dto);

        // Delete application (withdraw or admin remove)
        Task<bool> DeleteApplicationAsync(int id);
    }
}
