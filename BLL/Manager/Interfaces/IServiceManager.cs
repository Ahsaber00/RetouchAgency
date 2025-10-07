using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Manager.Interfaces
{
    public interface IServiceManager
    {
        // Get all services
        Task<IEnumerable<ServiceDTO>> GetAllServicesAsync();

        // Get a single service by Id
        Task<ServiceDTO?> GetServiceByIdAsync(int serviceId);

        // Create a new service
        Task<int> CreateServiceAsync(ServiceCreateDTO serviceCreateDto, int userId);

        // Update a service
        Task UpdateServiceAsync(int id, ServiceCreateDTO serviceUpdateDto, int requestingUserId);

        // Delete a service
        Task DeleteServiceAsync(int serviceId, int requestingUserId);

        // Get services by user
        Task<IEnumerable<ServiceDTO>> GetServicesByUserAsync(int userId);

        // Check if user owns the service
        Task<bool> IsUserServiceOwnerAsync(int serviceId, int userId);
    }
}