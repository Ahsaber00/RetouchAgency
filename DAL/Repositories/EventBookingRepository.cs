using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class EventBookingRepository(ApplicationContext context) : GenericRepository<EventBooking>(context),IEventBookingRepository
    {
        public readonly ApplicationContext _context = context;

        public Task<bool> CheckBookingExistsAsync(int eventId, int userId)
        {
            return _context.EventBookings
                .AnyAsync(b => b.EventId == eventId && b.UserId == userId);
        }

        public async Task<IEnumerable<EventBooking>> GetBookingsByEventIdAsync(int eventId)
        {
            return await _context.EventBookings
                .Where(b => b.EventId == eventId).ToListAsync();
        }

        public async Task<IEnumerable<EventBooking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _context.EventBookings
                .Where(b => b.UserId == userId).ToListAsync();
        }
    }
}
