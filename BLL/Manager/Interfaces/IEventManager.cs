using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Manager.Interfaces
{
    public interface IEventManager
    {
        // Get all events
        Task<IEnumerable<Event>> GetAllEventsAsync();

        // Get a single event by Id
        Task<Event> GetEventByIdAsync(int eventId);

        // Create a new event
        Task<Event> CreateEventAsync(Event newEvent);

        // Update an event
        Task<Event> UpdateEventAsync(Event updatedEvent);

        // Delete an event
        Task<bool> DeleteEventAsync(int eventId);

        // Book an event for a user
        Task<EventBooking> BookEventAsync(int eventId, int userId);

        // Cancel a booking
        Task<bool> CancelBookingAsync(int bookingId, int userId);

        // Get all bookings for an event
        Task<IEnumerable<EventBooking>> GetBookingsByEventAsync(int eventId);
    }
}
