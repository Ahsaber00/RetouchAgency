using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IEventBookingRepository : IGenericRepository<EventBooking>
    {
        public Task<IEnumerable<EventBooking>> GetBookingsByUserIdAsync(int userId);
        public Task<IEnumerable<EventBooking>> GetBookingsByEventIdAsync(int eventId);
        public Task<bool> CheckBookingExistsAsync(int eventId, int userId);
    }
}
