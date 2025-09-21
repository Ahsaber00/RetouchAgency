using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [ForeignKey("PostedByUser")]
        public int PostedByUserId { get; set; }

        [Required]
        public string Title { get; set; }

        public int Capacity { get; set; } // Max attendees

        public DateTime StartDatetime { get; set; }

        public DateTime EndDatetime { get; set; }

        public virtual User PostedByUser { get; set; }
        public virtual ICollection<EventBooking> EventBookings { get; set; }
    }
}
