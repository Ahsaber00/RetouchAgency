namespace BLL.Manager.Interfaces;

using BLL.DTOs;

interface IEventBookingManager
{
    Task BookEventAsync(EventBookingDTO bookingDto);
    Task CancelBookingAsync(int eventId, int userId);
    Task<IEnumerable<EventBookingResponseDTO>> GetAllEventBookingsAsync();
    Task<IEnumerable<EventBookingResponseDTO>> GetBookingsByUserIdAsync(int userId);
    Task<IEnumerable<EventBookingResponseDTO>> GetBookingsByEventIdAsync(int eventId);
}