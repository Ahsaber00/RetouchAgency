using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Opportunity
    {
        [Key]
        [Column("OpportunityId")]
        public int OpportunityId { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Type { get; set; } // 'Job', 'Internship'

        public string Status { get; set; } // 'Scheduled', 'Open', 'Closed'

        public DateTime ApplicationStartDate { get; set; }

        public DateTime ApplicationEndDate { get; set; }

        public virtual Department Department { get; set; }
        public virtual User PostedByUser { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
    }
}
