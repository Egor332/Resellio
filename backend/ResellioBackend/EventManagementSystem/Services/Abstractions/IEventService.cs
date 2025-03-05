using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Services.Abstractions;

public interface IEventService
{
    Task<ResultBase> CreateEventAsync(EventDto eventDto, int organiserId);
}