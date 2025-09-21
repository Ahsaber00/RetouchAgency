using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        public IGenericRepository<Service> Services { get; }

        public IGenericRepository<TeamMember> TeamMembers { get; }

        public IGenericRepository<Opportunity> Opportunities { get; }

        public IGenericRepository<Event> Events { get; }

        public IGenericRepository<Application> Applications { get; }

        public IGenericRepository<EventBooking> EventBookings { get; }

        public IGenericRepository<User> Users { get; }
        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
            Services = new ServiceRepository(context);
            TeamMembers = new TeamMemberRepository(context);
            Opportunities=new OpportunutyRepository(context);
            Events=new EventRepository(context);
            Applications=new ApplicationRepository(context);
            EventBookings=new EventBookingRepository(context);
            Users = new UserRepository(context);
        }

        public async Task<int> SaveAllAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
