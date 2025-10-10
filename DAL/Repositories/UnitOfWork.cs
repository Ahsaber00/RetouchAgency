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

        public IGenericRepository<Opportunity> Opportunities {get;}

        public IEventRepository Events {get;}

        public IApplicationRepository Applications {get;}

        public IEventBookingRepository EventBookings {get;}

        public IUserRepository Users {get;}

        public IUserRequestRepository UserRequests { get; }

        public ICourseRepository Courses { get; }

        public IUserRequestCourseRepository UserRequestedCourses { get; }

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
            UserRequests=new UserRequestRepository(context);
            Courses=new CourseRepository(context);
            UserRequestedCourses=new UserRequestCourseRepository(context);

        }

        public async Task<int> SaveAllAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
