using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.Results;

namespace ResellioBackend.EventManagementSystem.Creators.Abstractions;

public interface IEventCreatorService
{
    Task<ResultBase> CreateEventAsync(EventDto eventDto, int organiserId);
}