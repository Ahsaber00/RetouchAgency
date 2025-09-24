using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class OpportunityDto
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public int UserId { get; set; }
        public string PostedBy { get; set; }

        public string Title { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

        public DateTime ApplicationStartDate { get; set; }
        public DateTime ApplicationEndDate { get; set; }

        public OpportunityDto()
        {
            
        }

        public OpportunityDto(Opportunity op)
        {
            Id = op.Id;
            DepartmentId = op.DepartmentId;
            DepartmentName = op.Department.Name;
            UserId = op.UserId;
            PostedBy = op.PostedByUser.FirstName;
            Title = op.Title; 
            Type = op.Type;
            Status = op.Status;
            ApplicationStartDate = op.ApplicationStartDate;
            ApplicationEndDate = op.ApplicationEndDate;
        }
    }
}
