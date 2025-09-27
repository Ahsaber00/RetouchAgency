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

        [ForeignKey("Opportunity")]
        public int OpportunityId { get; set; }
        public string ApplicantPhoneNumber {  get; set; }
        public string ApplicantUniversity {  get; set; }
        public string ResumeUrl { get; set; }

        public string Status { get; set; } // 'Submitted', 'Reviewed', 'Hired'

        public virtual User User { get; set; }
        public virtual Opportunity Opportunity { get; set; }
    }
}
