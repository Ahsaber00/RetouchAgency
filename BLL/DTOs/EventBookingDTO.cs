namespace BLL.DTOs;

public class EventBookingDTO
{
    public int EventId { get; set; }
    public int UserId { get; set; }
    public DateTime BookingDate { get; internal set; } = DateTime.Now;
}

public class EventBookingResponseDTO
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int UserId { get; set; }
    public DateTime BookingDate { get; set; }
}
