using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string GoogleId { get; set; }

        public string AuthMethod { get; set; } // 'local', 'google'

        public string Role { get; set; } // 'admin', 'applicant'

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Application>? Applications { get; set; }
        public virtual ICollection<Opportunity>? Opportunities { get; set; }
        public virtual ICollection<Event>? Events { get; set; }
        public virtual ICollection<EventBooking>? EventBookings { get; set; }
    }
}