using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.Results;

namespace ResellioBackend.EventManagementSystem.Creators.Abstractions
{
    public interface IEventCreatorService
    {
        public Task<ResultBase> CreateEvent(EventDto eventDto, int organiserId);
    }
}