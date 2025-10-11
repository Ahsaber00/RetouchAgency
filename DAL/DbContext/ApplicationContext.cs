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
        public DbSet<Course> Courses { get; set; }
        public DbSet<UserRequest> UserRequests { get; set; }
        public DbSet<UserRequestCourse> UserRequestCourses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Department>().HasData(
               new Department { Id = 1, Name = "Camera & Video Editing" },
                new Department { Id = 2, Name = "Marketing" },
                new Department { Id = 3, Name = "Graphic Design" },
                new Department { Id = 4, Name = "Operation" },
                new Department { Id = 5, Name = "PR" },
                new Department { Id = 6, Name = "Sales" },
                new Department { Id = 7, Name = "Web Development" },
                new Department { Id = 8, Name = "HR" },
                new Department { Id = 9, Name = "Marketing Manager" }
            );


            modelBuilder.Entity<Opportunity>().HasData(
            new Opportunity
            {
                Id = 1,
                DepartmentId = 1, // Camera & Video Editing
                UserId = 4,
                Title = "Video Editing Internship",
                Type = "internship",
                Status = "open",
                ApplicationStartDate = new DateTime(2025, 9, 1),
                ApplicationEndDate = new DateTime(2025, 9, 20)
            },
            new Opportunity
            {
                Id = 2,
                DepartmentId = 2, // Marketing
                UserId = 4,
                Title = "Marketing Coordinator Job Offer",
                Type = "job offer",
                Status = "open",
                ApplicationStartDate = new DateTime(2025, 9, 5),
                ApplicationEndDate = new DateTime(2025, 9, 25)
            },
            new Opportunity
            {
                Id = 3,
                DepartmentId = 3, // Graphic Design
                UserId = 4,
                Title = "Graphic Design Internship",
                Type = "internship",
                Status = "open",
                ApplicationStartDate = new DateTime(2025, 9, 10),
                ApplicationEndDate = new DateTime(2025, 9, 30)
            },
            new Opportunity
            {
                Id = 4,
                DepartmentId = 4, // Operation
                UserId = 4,
                Title = "Operations Assistant Job Offer",
                Type = "job offer",
                Status = "open",
                ApplicationStartDate = new DateTime(2025, 9, 12),
                ApplicationEndDate = new DateTime(2025, 9, 28)
            },
            new Opportunity
            {
                Id = 5,
                DepartmentId = 5, // PR
                UserId = 4,
                Title = "PR Internship",
                Type = "internship",
                Status = "open",
                ApplicationStartDate = new DateTime(2025, 9, 15),
                ApplicationEndDate = new DateTime(2025, 10, 5)
            },
            new Opportunity
            {
                Id = 6,
                DepartmentId = 6, // Sales
                UserId = 4,
                Title = "Sales Executive Job Offer",
                Type = "job offer",
                Status = "open",
                ApplicationStartDate = new DateTime(2025, 10, 1),
                ApplicationEndDate = new DateTime(2025, 10, 20)
            },
            new Opportunity
            {
                Id = 7,
                DepartmentId = 7, // Web Development
                UserId = 4,
                Title = "Web Development Internship",
                Type = "internship",
                Status = "open",
                ApplicationStartDate = new DateTime(2025, 9, 3),
                ApplicationEndDate = new DateTime(2025, 9, 25)
            },
            new Opportunity
            {
                Id = 8,
                DepartmentId = 8, // HR
                UserId = 4,
                Title = "HR Specialist Job Offer",
                Type = "job offer",
                Status = "open",
                ApplicationStartDate = new DateTime(2025, 9, 12),
                ApplicationEndDate = new DateTime(2025, 9, 22)
            });

            modelBuilder.Entity<Service>().HasData(
                    new Service
                    {
                        Id = 1,
                        Name = "Marketing",
                        Description = "Comprehensive marketing services including digital campaigns, social media management, and promotional strategies.",
                        UserId = 15 // assuming seeded admin user or adjust later
                    },
                    new Service
                    {
                        Id = 2,
                        Name = "Brand Development",
                        Description = "Brand identity creation, logo design, and strategic brand positioning services.",
                        UserId = 15
                    }
            );

            modelBuilder.Entity<Course>().HasData(
                        new Course
                        {
                            Id = 1,
                            Name = "Digital Marketing Fundamentals",
                            Description = "Learn the essential skills for online marketing including SEO, SEM, and PPC.",
                            ServiceId = 1
                        },
                        new Course
                        {
                            Id = 2,
                            Name = "Social Media Marketing",
                            Description = "Develop effective strategies to grow businesses on Facebook, Instagram, and LinkedIn.",
                            ServiceId = 1
                        },
                        new Course
                        {
                            Id = 3,
                            Name = "Email Marketing Strategies",
                            Description = "Learn to design and execute email campaigns that convert leads into customers.",
                            ServiceId = 1
                        },
                        new Course
                        {
                            Id = 4,
                            Name = "Content Marketing & SEO",
                            Description = "Master content creation and optimization techniques to drive organic growth.",
                            ServiceId = 1
                        }
            );

        }

    }
}
