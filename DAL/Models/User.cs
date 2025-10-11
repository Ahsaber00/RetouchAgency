using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class User
    {
        [Key]
        [Column("UserId")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "applicant"; // 'admin', 'applicant'

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Application>? Applications { get; set; }
        public virtual ICollection<Opportunity>? Opportunities { get; set; }
        public virtual ICollection<Event>? Events { get; set; }
        public virtual ICollection<EventBooking>? EventBookings { get; set; }
    }
}