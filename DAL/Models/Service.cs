using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        [Required]

        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public required string Description { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }

        // Navigation property
        public virtual User? User { get; set; }
    }
}
