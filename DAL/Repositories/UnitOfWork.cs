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

        public IServiceRepository Services {get;}

        public ITeamMemberRepository TeamMembers {get;}

        public IOpportunityRepository Opportunities {get;}

        public IEventRepository Events {get;}

        public IApplicationRepository Applications {get;}

        public IEventBookingRepository EventBookings {get;}

        public IUserRepository Users {get;}
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
