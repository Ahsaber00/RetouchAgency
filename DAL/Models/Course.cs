using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }    

        [ForeignKey("Service")]
        public int? ServiceId { get; set; }
        public virtual Service? Service { get; set; }
        public virtual ICollection<UserRequestCourse> UserRequestCourses { get; set; }


    }
}
