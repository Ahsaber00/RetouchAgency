using BLL.DTOs;
using BLL.Manager.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Manager
{
    public class ServiceManager : IServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ServiceDTO>> GetAllServicesAsync()
        {
            var services = await _unitOfWork.Services.GetAllAsync();
            return services.Select(MapToServiceDTO).ToList();
        }

        public async Task<ServiceDTO?> GetServiceByIdAsync(int serviceId)
        {
            var serviceEntity = await _unitOfWork.Services.GetByIdAsync(serviceId);
            return serviceEntity == null ? null : MapToServiceDTO(serviceEntity);
        }

        public async Task<int> CreateServiceAsync(ServiceCreateDTO serviceCreateDto, int userId)
        {
            // Check if user exists
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found.");

            var serviceEntity = new Service
            {
                Name = serviceCreateDto.Name,
                Description = serviceCreateDto.Description,
                UserId = userId
            };

            await _unitOfWork.Services.AddAsync(serviceEntity);
            await _unitOfWork.SaveAllAsync();

            return serviceEntity.Id;
        }

        public async Task UpdateServiceAsync(int id, ServiceCreateDTO serviceUpdateDto, int requestingUserId)
        {
            var serviceEntity = await _unitOfWork.Services.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Service not found.");

            // Check if user is admin or service owner
            var requestingUser = await _unitOfWork.Users.GetByIdAsync(requestingUserId);
            if (requestingUser?.Role != UserRole.Admin && serviceEntity.UserId != requestingUserId)
                throw new UnauthorizedAccessException("You do not have permission to update this service.");

            serviceEntity.Name = serviceUpdateDto.Name;
            serviceEntity.Description = serviceUpdateDto.Description;

            _unitOfWork.Services.Update(serviceEntity);
            await _unitOfWork.SaveAllAsync();
        }

        public async Task DeleteServiceAsync(int serviceId, int requestingUserId)
        {
            var serviceEntity = await _unitOfWork.Services.GetByIdAsync(serviceId)
                ?? throw new KeyNotFoundException("Service not found.");

            // Check if user is admin or service owner
            var requestingUser = await _unitOfWork.Users.GetByIdAsync(requestingUserId);
            if (requestingUser?.Role != UserRole.Admin && serviceEntity.UserId != requestingUserId)
                throw new UnauthorizedAccessException("You do not have permission to delete this service.");

            await _unitOfWork.Services.DeleteAsync(serviceId);
            await _unitOfWork.SaveAllAsync();
        }

        public async Task<IEnumerable<ServiceDTO>> GetServicesByUserAsync(int userId)
        {
            var services = await _unitOfWork.Services.GetAllAsync();
            var userServices = services.Where(s => s.UserId == userId);
            return userServices.Select(MapToServiceDTO).ToList();
        }

        public async Task<bool> IsUserServiceOwnerAsync(int serviceId, int userId)
        {
            var serviceEntity = await _unitOfWork.Services.GetByIdAsync(serviceId);
            return serviceEntity?.UserId == userId;
        }

        private ServiceDTO MapToServiceDTO(Service serviceEntity)
        {
            return new ServiceDTO
            {
                Id = serviceEntity.Id,
                Name = serviceEntity.Name,
                Description = serviceEntity.Description,
                UserId = serviceEntity.UserId,
                UserName = serviceEntity.User?.FirstName + " " + serviceEntity.User?.LastName,
                UserEmail = serviceEntity.User?.Email
            };
        }
    }
}