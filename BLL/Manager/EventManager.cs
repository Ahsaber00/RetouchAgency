using BLL.DTOs;
using BLL.Manager.Interfaces;
using BLL.Services;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Http;

namespace BLL.Manager;
public class EventManager(IUnitOfWork unitOfWork, IFileUploadService fileUploadService) : IEventManager
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileUploadService _fileUploadService = fileUploadService;

    public async Task<IEnumerable<EventDTO>> GetAllEventsAsync()
    {
        return (await _unitOfWork.Events.GetAllAsync()).Select(MapToEventDTO).ToList();
    }

    public async Task<EventDTO?> GetEventByIdAsync(int eventId)
    {
        var eventEntity = await _unitOfWork.Events.GetByIdAsync(eventId);
        return eventEntity == null ? null : MapToEventDTO(eventEntity);
    }

    public async Task<int> CreateEventAsync(EventCreateDTO eventCreateDto, int postedByUserId)
    {
        // Validate dates
        if (eventCreateDto.StartDatetime >= eventCreateDto.EndDatetime)
            throw new ArgumentException("End date must be after start date.");

        if (eventCreateDto.StartDatetime <= DateTime.UtcNow)
            throw new ArgumentException("Event start date must be in the future.");

        var eventEntity = new Event
        {
            Title = eventCreateDto.Title,
            Capacity = eventCreateDto.Capacity,
            StartDatetime = eventCreateDto.StartDatetime,
            EndDatetime = eventCreateDto.EndDatetime,
            PostedByUserId = postedByUserId
        };

        await _unitOfWork.Events.AddAsync(eventEntity);
        await _unitOfWork.SaveAllAsync();

        return eventEntity.Id;
    }

    public async Task UpdateEventAsync(int id, EventCreateDTO eventUpdateDto, int requestingUserId)
    {
        var eventEntity = await _unitOfWork.Events.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Event not found.");

        // Check if user is admin or event owner
        var requestingUser = await _unitOfWork.Users.GetByIdAsync(requestingUserId);
        if (requestingUser?.Role != UserRole.Admin && eventEntity.PostedByUserId != requestingUserId)
            throw new UnauthorizedAccessException("You do not have permission to update this event.");

        // Validate dates
        if (eventUpdateDto.StartDatetime >= eventUpdateDto.EndDatetime)
            throw new ArgumentException("End date must be after start date.");

        if (eventUpdateDto.StartDatetime <= DateTime.UtcNow)
            throw new ArgumentException("Event start date must be in the future.");

        eventEntity.Title = eventUpdateDto.Title;
        eventEntity.Capacity = eventUpdateDto.Capacity;
        eventEntity.StartDatetime = eventUpdateDto.StartDatetime;
        eventEntity.EndDatetime = eventUpdateDto.EndDatetime;

        _unitOfWork.Events.Update(eventEntity);
        await _unitOfWork.SaveAllAsync();
    }

    public async Task DeleteEventAsync(int eventId, int requestingUserId)
    {
        var eventEntity = await _unitOfWork.Events.GetByIdAsync(eventId)
            ?? throw new KeyNotFoundException("Event not found.");

        // Check if user is admin or event owner
        var requestingUser = await _unitOfWork.Users.GetByIdAsync(requestingUserId);
        if (requestingUser?.Role != UserRole.Admin && eventEntity.PostedByUserId != requestingUserId)
            throw new UnauthorizedAccessException("You do not have permission to delete this event.");

        await _unitOfWork.Events.DeleteAsync(eventId);
        await _unitOfWork.SaveAllAsync();
    }

    public async Task<IEnumerable<EventDTO>> GetEventsByUserAsync(int userId)
    {
        return (await _unitOfWork.Events.GetAllAsync())
            .Where(e => e.PostedByUserId == userId)
            .Select(MapToEventDTO).ToList();

    }

    public async Task<bool> IsUserEventOwnerAsync(int eventId, int userId)
    {
        return (await _unitOfWork.Events.GetByIdAsync(eventId))?.PostedByUserId == userId;
    }

    public async Task<string> UploadEventCoverImageAsync(int eventId, IFormFile imageFile, int requestingUserId)
    {
        // Check if event exists and user has permission
        var eventEntity = await _unitOfWork.Events.GetByIdAsync(eventId);
        if (eventEntity == null)
            throw new KeyNotFoundException("Event not found.");

        // Check if user is owner or admin (you can add admin check here)
        if (eventEntity.PostedByUserId != requestingUserId)
            throw new UnauthorizedAccessException("You can only upload images for your own events.");

        // Delete old image if exists
        if (!string.IsNullOrEmpty(eventEntity.CoverImageUrl))
        {
            await _fileUploadService.DeleteEventImageAsync(eventEntity.CoverImageUrl);
        }

        // Upload new image
        var imageUrl = await _fileUploadService.UploadEventImageAsync(imageFile);

        // Update event with new image URL
        eventEntity.CoverImageUrl = imageUrl;
        _unitOfWork.Events.Update(eventEntity);
        await _unitOfWork.SaveAllAsync();

        return imageUrl;
    }

    public async Task UpdateEventCoverImageAsync(int eventId, string imageUrl, int requestingUserId)
    {
        // Check if event exists and user has permission
        var eventEntity = await _unitOfWork.Events.GetByIdAsync(eventId)
            ?? throw new KeyNotFoundException("Event not found.");

        // Check if user is owner or admin (you can add admin check here)
        if (eventEntity.PostedByUserId != requestingUserId)
            throw new UnauthorizedAccessException("You can only update images for your own events.");

        // Delete old image if exists and different from new one
        if (!string.IsNullOrEmpty(eventEntity.CoverImageUrl) && eventEntity.CoverImageUrl != imageUrl)
        {
            await _fileUploadService.DeleteEventImageAsync(eventEntity.CoverImageUrl);
        }

        // Update event with new image URL
        eventEntity.CoverImageUrl = imageUrl;
        _unitOfWork.Events.Update(eventEntity);
        await _unitOfWork.SaveAllAsync();
    }

    private EventDTO MapToEventDTO(Event eventEntity)
    {
        var bookingsCount = eventEntity.EventBookings?.Count ?? 0;
        return new EventDTO
        {
            Id = eventEntity.Id,
            Title = eventEntity.Title,
            Capacity = eventEntity.Capacity,
            StartDatetime = eventEntity.StartDatetime,
            EndDatetime = eventEntity.EndDatetime,
            PostedByUserId = eventEntity.PostedByUserId,
            PostedByUserName = eventEntity.PostedByUser?.FirstName + " " + eventEntity.PostedByUser?.LastName,
            CoverImageUrl = eventEntity.CoverImageUrl,
            BookingsCount = bookingsCount,
            AvailableSlots = eventEntity.Capacity - bookingsCount
        };
    }
}