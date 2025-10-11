using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class UserRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status {  get; set; }
        public List<RequestedCoursesDto> Courses { get; set; }

        public UserRequestDto()
        {
            
        }

        public UserRequestDto(UserRequest userRequest)
        {
            Id = userRequest.Id;
            Name = $"{userRequest.User.FirstName + userRequest.User.LastName}";
            PhoneNumber = userRequest.PhoneNumber;
            CreatedAt = userRequest.CreatedAt;
            Courses = userRequest.UserRequestCourses.Select(urc => new RequestedCoursesDto
            {
                CourseId = urc.CourseId,
                CourseName = urc.Course.Name,
            }).ToList();
        }
    }
}
