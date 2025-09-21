using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext>options):base(options) { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventBooking> EventBookings { get; set; }
        public DbSet<Service> Services { get; set; }



    }
}
