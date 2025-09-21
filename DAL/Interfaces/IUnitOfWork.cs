using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Service> Services { get; }
        IGenericRepository<TeamMember> TeamMembers { get; }
        IGenericRepository<Opportunity> Opportunities { get; }
        IGenericRepository<Event> Events { get; }
        IGenericRepository<Application> Applications { get; }
        IGenericRepository<EventBooking> EventBookings { get; }
        IGenericRepository<User> Users { get; }

        Task<int> SaveAllAsync();
    }
}
