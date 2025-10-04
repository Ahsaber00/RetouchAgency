using BLL.DTOs;
using DAL.Models;
using Microsoft.AspNetCore.Http;
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
        Task<IEnumerable<EventDTO>> GetAllEventsAsync();

        // Get a single event by Id
        Task<EventDTO?> GetEventByIdAsync(int eventId);

        // Create a new event
        Task<int> CreateEventAsync(EventCreateDTO eventCreateDto, int postedByUserId);

        // Update an event
        Task UpdateEventAsync(int id, EventCreateDTO eventUpdateDto, int requestingUserId);

        // Delete an event
        Task DeleteEventAsync(int eventId, int requestingUserId);

        // Get events by user
        Task<IEnumerable<EventDTO>> GetEventsByUserAsync(int userId);

        // Check if user owns the event
        Task<bool> IsUserEventOwnerAsync(int eventId, int userId);

        // Upload cover image for event
        Task<string> UploadEventCoverImageAsync(int eventId, IFormFile imageFile, int requestingUserId);

        // Update event cover image
        Task UpdateEventCoverImageAsync(int eventId, string imageUrl, int requestingUserId);
    }
}
