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
    public class ApplicationManager : IApplicationManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApplicationManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApplicationDto> CreateApplicationAsync(CreateApplicationDto dto, int userId)
        {

            string resumeUrl = null;
            var opportunity=await _unitOfWork.Opportunities.GetByIdAsync(dto.OpportunityId);
            if(dto.ResumeFile!= null && dto.ResumeFile.Length>0)
            {
                var opportunityResumes = Path.Combine("wwwroot/resumes", opportunity.Title);
                if(!Directory.Exists(opportunityResumes))
                {
                    Directory.CreateDirectory(opportunityResumes);
                }

                var fileName= $"{Guid.NewGuid()}_{dto.ResumeFile.FileName}";
                var filePath=Path.Combine(opportunityResumes, fileName);
                using(var stream=new FileStream(filePath,FileMode.Create))
                {
                    await dto.ResumeFile.CopyToAsync(stream);
                }

                resumeUrl = $"/resumes/{opportunity.Title}/{fileName}";
            }


            Application application = new Application
            {
                UserId = userId,
                OpportunityId = dto.OpportunityId,
                ApplicantPhoneNumber = dto.PhoneNumber,
                ApplicantUniversity = dto.University.Trim().ToLower(),
                ResumeUrl = resumeUrl,
                Status = "Submited",
            };

            await _unitOfWork.Applications.AddAsync(application);
            await _unitOfWork.SaveAllAsync();
            var newApplication=await _unitOfWork.Applications.GetByIdAsync(application.Id,a=>a.User,application=>application.Opportunity);
            return new ApplicationDto(newApplication);

          
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            var application = await _unitOfWork.Applications.GetByIdAsync(id);
            if (application == null)
            {
                return false;
            }
            
            if(!string.IsNullOrWhiteSpace(application.ResumeUrl))
            {
                var filePath=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", application.ResumeUrl.TrimStart('/'));
                if(File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            await _unitOfWork.Applications.DeleteAsync(id);
            await _unitOfWork.SaveAllAsync();
            return true;
        }

        public async Task<IEnumerable<ApplicationDto>> GetAllApplicationsAsync(string? status = null)
        {
            var applications = _unitOfWork.Applications.Query();
            
            if(!string.IsNullOrWhiteSpace(status))
            {
                var normalizedStatus = status.Trim().ToLower();
                applications =applications.Where(a=>a.Status.ToLower() == normalizedStatus);
            }
            var result= await applications.Select(application=>new ApplicationDto
            {
                ApplicationId = application.Id,
                UserId = application.User.Id,
                UserFullName = $"{application.User.FirstName + application.User.LastName}",
                UserEmail = application.User.Email,
                PhoneNumber = application.ApplicantPhoneNumber,
                University = application.ApplicantUniversity,
                OpportunityId = application.OpportunityId,
                OpportunityTitle = application.Opportunity.Title,
                ResumeUrl = application.ResumeUrl,
                Status = application.Status,

            }).AsNoTracking().ToListAsync();

            return result;
        }

        public async Task<ApplicationDto?> GetApplicationByIdAsync(int id)
        {
            var application=await _unitOfWork.Applications.GetByIdAsync(id);
            if(application!=null)
            {
                return new ApplicationDto(application);
            }
            return null;
        }

        public async Task<IEnumerable<ApplicationDto>> GetApplicationsByOpportunityAsync(int opportunityId)
        {
            var applications= _unitOfWork.Applications.Query();
            var opportunityApplications=applications.Where(a=>a.OpportunityId==opportunityId);
            var result = await applications.Select(application => new ApplicationDto
            {
                ApplicationId = application.Id,
                UserId = application.User.Id,
                UserFullName = $"{application.User.FirstName + application.User.LastName}",
                UserEmail = application.User.Email,
                PhoneNumber = application.ApplicantPhoneNumber,
                University = application.ApplicantUniversity,
                OpportunityId = application.OpportunityId,
                OpportunityTitle = application.Opportunity.Title,
                ResumeUrl = application.ResumeUrl,
                Status = application.Status,

            }).AsNoTracking().ToListAsync();

            return result;

        }

        public async Task<IEnumerable<ApplicationDto>> GetApplicationsByUserAsync(int userId)
        {
            var applications = _unitOfWork.Applications.Query();
            var userApplications = applications.Where(a => a.UserId == userId);
            var result = await applications.Select(application => new ApplicationDto
            {
                ApplicationId = application.Id,
                UserId = application.User.Id,
                UserFullName = $"{application.User.FirstName + application.User.LastName}",
                UserEmail = application.User.Email,
                PhoneNumber = application.ApplicantPhoneNumber,
                University = application.ApplicantUniversity,
                OpportunityId = application.OpportunityId,
                OpportunityTitle = application.Opportunity.Title,
                ResumeUrl = application.ResumeUrl,
                Status = application.Status,

            }).AsNoTracking().ToListAsync();

            return result;
        }

        public async Task<ApplicationDto?> UpdateApplicationAsync(int id, UpdateApplicationDto dto)
        {
            var application= await _unitOfWork.Applications.GetByIdAsync(id);
            if(application == null)
            {
                return null;
            }
            application.Status=dto.Status.Trim().ToLower() ?? application.Status;
            _unitOfWork.Applications.Update(application);
            var result = await _unitOfWork.Applications.GetByIdAsync(application.Id, a => a.User, application => application.Opportunity);
            await _unitOfWork.SaveAllAsync();
            return new ApplicationDto(application);
        }
    }
}
