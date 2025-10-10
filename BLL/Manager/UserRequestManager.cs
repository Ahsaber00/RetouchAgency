using BLL.DTOs;
using BLL.Manager.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Manager
{
    public class UserRequestManager : IUserRequestsManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserRequestManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserRequestDto> CreateRequestAsync(int userId, CreateUserRequestDto dto)
        {
            // Step 1: Create and save the main UserRequest first
            var newRequest = new UserRequest
            {
                UserId = userId,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.UserRequests.AddAsync(newRequest);
            await _unitOfWork.SaveAllAsync();

            // Step 2: Now that we have the newRequest.Id, create UserRequestCourses
            var userRequestCourses = dto.CourseIds.Select(courseId => new UserRequestCourse
            {
                UserRequestId = newRequest.Id,  
                CourseId = courseId,
                UpdatedAt = DateTime.UtcNow
            }).ToList();

            foreach (var urc in userRequestCourses)
                await _unitOfWork.UserRequestedCourses.AddAsync(urc);

            await _unitOfWork.SaveAllAsync();

            
            var result = await _unitOfWork.UserRequests.GetByIdAsync(
                newRequest.Id,
                r => r.User,
                r => r.UserRequestCourses
              
            );

            return new UserRequestDto(result);
        }


        public async Task<bool> DeleteRequestAsync(int id)
        {
            var userRequest = await _unitOfWork.UserRequests.GetByIdAsync(id);
            if(userRequest == null)
            {
                return false;
            }

            var relatedCourses = userRequest.UserRequestCourses.ToList();

            foreach (var urc in relatedCourses)
            {
                await _unitOfWork.UserRequestedCourses.DeleteAsync(urc.Id);
            }

            await _unitOfWork.UserRequests.DeleteAsync(id);
      
            
            return true;
            
        }

        public async Task<IEnumerable<UserRequestDto>> GetRequestsAsync()
        {
            var requests =await _unitOfWork.UserRequests.GetAllAsync(r=>r.UserRequestCourses);
            var result = requests.Select(r => new UserRequestDto
            {
                Id = r.Id,
                Name = $"{r.User.FirstName + r.User.LastName}",
                PhoneNumber = r.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                Status = r.Status,
                Courses = r.UserRequestCourses.Select(urc => new RequestedCoursesDto
                {
                    CourseId = urc.CourseId,
                    CourseName = urc.Course.Name
                }).ToList(),
            });
            return result;
        }

        public async Task<bool> UpdateRequestStatusAsync(int id,UpdateRequestStatusDto dto)
        {
            var request= await _unitOfWork.UserRequests.GetByIdAsync(id);
            if(request == null)
            {
                return false;
            }
            request.Status= dto.Status.Trim().ToLower()??request.Status;
            _unitOfWork.UserRequests.Update(request);
            await _unitOfWork.SaveAllAsync();
            return true;
        }
    }
}
