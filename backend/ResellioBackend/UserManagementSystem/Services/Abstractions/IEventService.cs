using ResellioBackend.UserManagementSystem.DTOs;
using ResellioBackend.UserManagementSystem.Models;

namespace ResellioBackend.UserManagementSystem.Services.Abstractions;

public interface IEventService
{
    Task<Event> CreateEventAsync(EventDTO eventDto);
}