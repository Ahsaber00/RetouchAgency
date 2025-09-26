namespace BLL.Manager;
using BLL.DTOs;
using BLL.Manager.Interfaces;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

public class EventBookingManager(ApplicationContext context) : IEventBookingManager
{
    private readonly ApplicationContext _context = context;

    public async Task<IEnumerable<EventBookingResponseDTO>> GetAllEventBookingsAsync()
    {
        var bookings = await _context.EventBookings.ToListAsync();
        return bookings.Select(b => new EventBookingResponseDTO
        {
            Id = b.Id,
            UserId = (int) b.UserId,
            EventId = b.EventId,
            BookingDate = b.BookingDate,
        });
    }

    public async Task<EventBookingResponseDTO?> GetEventBookingByIdAsync(int id)
    {
        var booking = await _context.EventBookings.FindAsync(id);
        if (booking == null) return null;

        return new EventBookingResponseDTO
        {
            Id = booking.Id,
            UserId = (int)booking.UserId,
            EventId = booking.EventId,
            BookingDate = booking.BookingDate
        };
    }

    public async Task BookEventAsync(EventBookingDTO bookingDTO)
    {
        var booking = new EventBooking
        {
            UserId = bookingDTO.UserId,
            EventId = bookingDTO.EventId,
        };

        _context.EventBookings.Add(booking);
        await _context.SaveChangesAsync();
        return;
    }



    public Task<IEnumerable<EventBookingResponseDTO>> GetBookingsByUserIdAsync(int userId)
    {
        return _context.EventBookings
            .Where(b => b.UserId == userId)
            .Select(b => new EventBookingResponseDTO
            {
                Id = b.Id,
                UserId = (int)b.UserId,
                EventId = b.EventId,
                BookingDate = b.BookingDate
            })
            .ToListAsync()
            .ContinueWith(task => task.Result.AsEnumerable());
    }

    public Task<IEnumerable<EventBookingResponseDTO>> GetBookingsByEventIdAsync(int eventId)
    {
        return _context.EventBookings
            .Where(b => b.EventId == eventId)
            .Select(b => new EventBookingResponseDTO
            {
                Id = b.Id,
                UserId = (int)b.UserId,
                EventId = b.EventId,
                BookingDate = b.BookingDate
            })
            .ToListAsync()
            .ContinueWith(task => task.Result.AsEnumerable());
    }

    public async Task CancelBookingAsync(int eventId, int userId)
    {
        var booking = await _context.EventBookings
            .FirstOrDefaultAsync(b => b.EventId == eventId && b.UserId == userId)
            ?? throw new InvalidOperationException("Booking not found.");
            
        _context.EventBookings.Remove(booking);
        await _context.SaveChangesAsync();
        
    }
}