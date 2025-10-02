using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs;


public class EventDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Capacity is required")]
    [Range(1, 10000, ErrorMessage = "Capacity must be between 1 and 10000")]
    public int Capacity { get; set; }

    [Required(ErrorMessage = "Start date and time is required")]
    public DateTime StartDatetime { get; set; }

    [Required(ErrorMessage = "End date and time is required")]
    public DateTime EndDatetime { get; set; }

    public int PostedByUserId { get; set; }

    // Read-only properties for response
    public string? PostedByUserName { get; set; }
    public int AvailableSlots { get; set; }
    public int BookingsCount { get; set; }
}

public class EventCreateDTO
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Capacity is required")]
    [Range(1, 10000, ErrorMessage = "Capacity must be between 1 and 10000")]
    public int Capacity { get; set; }

    [Required(ErrorMessage = "Start date and time is required")]
    public DateTime StartDatetime { get; set; }

    [Required(ErrorMessage = "End date and time is required")]
    public DateTime EndDatetime { get; set; }

    // PostedByUserId will be set from the authenticated user's claims
}