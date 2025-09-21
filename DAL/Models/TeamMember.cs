using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class TeamMember
    {
        [Key]
        public int MemberId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        public string Position { get; set; }

        public bool IsHead { get; set; }

        public virtual User User { get; set; }
        public virtual Department Department { get; set; }

    }
}
