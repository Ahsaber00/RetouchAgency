namespace BLL.Manager.Interfaces;

using BLL.DTOs;

public interface IEventBookingManager
{
    Task BookEventAsync(EventBookingDTO bookingDto);
    Task CancelBookingAsync(int bookingId);
    Task<EventBookingResponseDTO?> GetEventBookingByIdAsync(int id);
    Task<IEnumerable<EventBookingResponseDTO>> GetAllEventBookingsAsync();
    Task<IEnumerable<EventBookingResponseDTO>> GetBookingsByUserIdAsync(int userId);
    Task<IEnumerable<EventBookingResponseDTO>> GetBookingsByEventIdAsync(int eventId);
}