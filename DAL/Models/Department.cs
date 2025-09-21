using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsHead { get; set; }
        public virtual ICollection<TeamMember> TeamMembers { get; set; }
        public virtual ICollection<Opportunity> Opportunities { get; set; }
    }
}
