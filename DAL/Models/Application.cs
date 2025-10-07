using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Application
    {
        [Key]
        [Column("ApplicationId")]
        public int Id { get; set; }

        
        [ForeignKey("User")]
        public int ? UserId { get; set; }   
        [Required]
        [ForeignKey("Opportunity")]
        public int OpportunityId { get; set; }

        [Required]
        [Phone]
        [MaxLength(20)] 
        public string ApplicantPhoneNumber { get; set; }

        [Required]
        [StringLength(150)]
        public string ApplicantUniversity { get; set; }

        [Required]
        [MaxLength(500)] 
        public string ResumeUrl { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Submitted"; // default status

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Opportunity Opportunity { get; set; }
    }

}
