using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class UserRequestCourse
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserRequest")]
        public int UserRequestId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public virtual Course Course { get; set; }
        public virtual UserRequest UserRequest { get; set; }
    }
}
