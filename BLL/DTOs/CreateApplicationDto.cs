using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class CreateApplicationDto
    {
        public int UserId { get; set; }
        public int OpportunityId { get; set; }
        public IFormFile ResumeFile { get; set; }

        // Extra fields from form
        public string PhoneNumber { get; set; }
        public string University { get; set; }
    }
}
