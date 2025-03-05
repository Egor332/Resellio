using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.UserManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Services.Abstractions;

public interface IEventService
{
    Task<Event> CreateEventAsync(EventDto eventDto, int organiserId);
}