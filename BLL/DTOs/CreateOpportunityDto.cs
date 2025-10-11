using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class CreateOpportunityDto
    {
        public int DepartmentId { get; set; }

        public string Title { get; set; }
        public string Type { get; set; }   // Job / Internship
        public string Status { get; set; } // Open / Closed / Scheduled

        public DateTime ApplicationStartDate { get; set; }
        public DateTime ApplicationEndDate { get; set; }
    }
}
