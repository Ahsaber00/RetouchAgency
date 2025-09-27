namespace BLL.Manager;
using BLL.DTOs;
using BLL.Manager.Interfaces;
using DAL;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

public class EventBookingManager(IUnitOfWork context) : IEventBookingManager
{
    private readonly IUnitOfWork _context = context;

    public async Task<IEnumerable<EventBookingResponseDTO>> GetAllEventBookingsAsync()
    {
        var bookings = await _context.EventBookings.GetAllAsync();
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
        var booking = await _context.EventBookings.GetByIdAsync(id);
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
        if (await _context.EventBookings.CheckBookingExistsAsync(bookingDTO.EventId, bookingDTO.UserId))
            throw new InvalidOperationException("User has already booked this event.");
        var booking = new EventBooking
        {
            UserId = bookingDTO.UserId,
            EventId = bookingDTO.EventId,
        };

        await _context.EventBookings.AddAsync(booking);
        return;
    }



    public Task<IEnumerable<EventBookingResponseDTO>> GetBookingsByUserIdAsync(int userId)
    {
        return _context.EventBookings
            .GetBookingsByUserIdAsync(userId)
            .ContinueWith(task => task.Result.Select(b => new EventBookingResponseDTO
            {
                Id = b.Id,
                UserId = (int)b.UserId,
                EventId = b.EventId,
                BookingDate = b.BookingDate
            }));
    }

    public Task<IEnumerable<EventBookingResponseDTO>> GetBookingsByEventIdAsync(int eventId)
    {
        return _context.EventBookings
            .GetBookingsByEventIdAsync(eventId)
            .ContinueWith(task => task.Result.Select(b => new EventBookingResponseDTO
            {
                Id = b.Id,
                UserId = (int)b.UserId,
                EventId = b.EventId,
                BookingDate = b.BookingDate
            }));
    }

    public async Task CancelBookingAsync(int bookingId)
    {
        var booking = await _context.EventBookings.GetByIdAsync(bookingId)
            ?? throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");
        await _context.EventBookings.DeleteAsync(bookingId);
    }
}