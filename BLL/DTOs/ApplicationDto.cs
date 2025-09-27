using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class ApplicationDto
    {
        public int ApplicationId { get; set; }

        // From User
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string PhoneNumber { get; set; }   
        public string University { get; set; }   

        // From Opportunity
        public int OpportunityId { get; set; }
        public string OpportunityTitle { get; set; }

        // From Application
        public string ResumeUrl { get; set; }
        public string Status { get; set; }

        public ApplicationDto() { }
        
         

        public ApplicationDto(Application application)
        {
            ApplicationId = application.Id;
            UserId = application.UserId.Value;
            OpportunityId = application.OpportunityId;
            UserFullName = $"{application.User.FirstName + application.User.LastName}";
            UserEmail=application.User.Email;
            PhoneNumber=application.ApplicantPhoneNumber;
            University=application.ApplicantUniversity;
            OpportunityTitle = application.Opportunity.Title;
            ResumeUrl = application.ResumeUrl;
            Status = application.Status;
        }
    }
}
