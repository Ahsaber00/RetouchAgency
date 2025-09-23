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
        IServiceRepository Services { get; }
        ITeamMemberRepository TeamMembers { get; }
        IGenericRepository<Opportunity> Opportunities { get; }
        IEventRepository Events { get; }
        IApplicationRepository Applications { get; }
        IEventBookingRepository EventBookings { get; }
        IUserRepository Users { get; }

        Task<int> SaveAllAsync();
    }
}
