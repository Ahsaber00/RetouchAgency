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
    public class OpportunityManager : IOpportunityManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public OpportunityManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<OpportunityDto> CreateOpportunityAsync(CreateOpportunityDto dto,int userId)
        {
            var newOpportunity = new Opportunity
            {
                DepartmentId = dto.DepartmentId,
                UserId = userId,
                Title = dto.Title,
                Type = dto.Type.Trim().ToLower(),
                Status = dto.Status.Trim().ToLower(),
                ApplicationStartDate = dto.ApplicationStartDate,
                ApplicationEndDate = dto.ApplicationEndDate
            };

            await _unitOfWork.Opportunities.AddAsync(newOpportunity);
            await _unitOfWork.SaveAllAsync();

            return new OpportunityDto(newOpportunity);
            
        }

        public async Task<bool> DeleteOpportunityAsync(int id)
        {
            var opportunity= await _unitOfWork.Opportunities.GetByIdAsync(id);
            if(opportunity == null)
            {
                return false;
            }

           await _unitOfWork.Opportunities.DeleteAsync(id);
           return true;
        }

        public async Task<IEnumerable<OpportunityDto>> GetAllOpportunitiesAsync(string? opportunityType = null)
        {
            var opportunities = _unitOfWork.Opportunities.Query();
            if (!String.IsNullOrWhiteSpace(opportunityType))
            {
                var normalizedType = opportunityType.Trim().ToLower();
                opportunities = opportunities.Where(o => o.Type.Trim().ToLower() == normalizedType);
            }

            var opportunitiesDto =await opportunities.Select(o => new OpportunityDto
            {
                Id = o.Id,
                DepartmentId = o.DepartmentId,
                DepartmentName = o.Department != null ? o.Department.Name : null,
                UserId = o.UserId,
                PostedBy = o.PostedByUser != null ? o.PostedByUser.FirstName : null,
                Title = o.Title,
                Type = o.Type,
                Status = o.Status,
                ApplicationStartDate = o.ApplicationStartDate,
                ApplicationEndDate = o.ApplicationEndDate
            }).AsNoTracking().ToListAsync();


            return opportunitiesDto;
        }
            

        public async Task<OpportunityDto> GetOpportunityByIdAsync(int id)
        {
            var opportunity = await _unitOfWork.Opportunities.GetByIdAsync(id);
            if (opportunity == null)
            {
                return null;
            }

            return new OpportunityDto(opportunity);
            
        }

        public async Task<OpportunityDto> UpdateOpportunityAsync(int id, UpdateOpportunityDto dto)
        {
            var opportunity = await _unitOfWork.Opportunities.GetByIdAsync(id);
            if (opportunity == null)
            {
                return null; 
            }

            opportunity.Title = dto.Title ?? opportunity.Title;
            opportunity.Type = dto.Type.Trim().ToLower() ?? opportunity.Type;
            opportunity.Status = dto.Status.Trim().ToLower() ?? opportunity.Status;
            opportunity.ApplicationStartDate = dto.ApplicationStartDate;
            opportunity.ApplicationEndDate = dto.ApplicationEndDate;
            opportunity.DepartmentId = dto.DepartmentId;
            

            _unitOfWork.Opportunities.Update(opportunity);
            await _unitOfWork.SaveAllAsync();

            return new OpportunityDto(opportunity);
            
        }
    }
}
