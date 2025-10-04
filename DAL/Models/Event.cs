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
        [Column("EventId")]
        public int Id { get; set; }

        [ForeignKey("PostedByUser")]
        public int PostedByUserId { get; set; }

        [Required]
        public required string Title { get; set; }

        public int Capacity { get; set; } // Max attendees

        public DateTime StartDatetime { get; set; }

        public DateTime EndDatetime { get; set; }

        [StringLength(500)]
        public string? CoverImageUrl { get; set; }

        public virtual User? PostedByUser { get; set; }
        public virtual ICollection<EventBooking> EventBookings { get; set; } = new List<EventBooking>();
    }
}
