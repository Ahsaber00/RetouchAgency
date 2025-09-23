using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dto
{
    public class UpdateOpportunityDto
    {
        public string Title { get; set; }
        public string Type { get; set; }   // Job / Internship
        public string Status { get; set; } // Open / Closed / Scheduled
        public int DepartmentId { get; set; }
        public DateTime ApplicationStartDate { get; set; }
        public DateTime ApplicationEndDate { get; set; }
    }
}
