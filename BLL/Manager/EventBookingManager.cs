namespace BLL.Manager;
using BLL.DTOs;
using BLL.Manager.Interfaces;
using DAL;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

public class EventBookingManager(IUnitOfWork unitOfWork) : IEventBookingManager
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<EventBookingResponseDTO>> GetAllEventBookingsAsync()
    {
        var bookings = await _unitOfWork.EventBookings.GetAllAsync();
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
        var booking = await _unitOfWork.EventBookings.GetByIdAsync(id);
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
        if (await _unitOfWork.EventBookings.CheckBookingExistsAsync(bookingDTO.EventId, bookingDTO.UserId))
            throw new InvalidOperationException("User has already booked this event.");
        var eventEntity = await _unitOfWork.Events.GetByIdAsync(bookingDTO.EventId)
            ?? throw new KeyNotFoundException($"Event with ID {bookingDTO.EventId} does not exist.");
        if (eventEntity.Capacity <= 0)
            throw new InvalidOperationException("Event is fully booked.");
        var booking = new EventBooking
        {
            UserId = bookingDTO.UserId,
            EventId = bookingDTO.EventId,
        };
        await _unitOfWork.EventBookings.AddAsync(booking);
        eventEntity.Capacity--;
        _unitOfWork.Events.Update(eventEntity);
        
        return;
    }



    public Task<IEnumerable<EventBookingResponseDTO>> GetBookingsByUserIdAsync(int userId)
    {
        return _unitOfWork.EventBookings
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
        return _unitOfWork.EventBookings
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
        var booking = await _unitOfWork.EventBookings.GetByIdAsync(bookingId)
            ?? throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");
        await _unitOfWork.EventBookings.DeleteAsync(bookingId);
    }
}