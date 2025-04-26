using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.Common.Paging;
using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Filtering;
using ResellioBackend.EventManagementSystem.RequestsParameters;
using ResellioBackend.EventManagementSystem.Services.Abstractions;
using ResellioBackend.UserManagementSystem.Statics;

namespace ResellioBackend.EventManagementSystem.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventCreatorService _eventCreatorService;
        private readonly IEventService _eventService;
        
        public EventsController(IEventCreatorService eventCreatorService, IEventService eventService)
        {
            _eventCreatorService = eventCreatorService;
            _eventService = eventService;
        }

        [Authorize(Policy = AuthorizationPolicies.OrganiserPolicy)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateEventAsync([FromBody] EventDto eventDto)
        {
            var organiserIdClaim = User.FindFirst(BearerTokenClaimsNames.Id);
            if (organiserIdClaim == null)
            {
                return Unauthorized(new { Message = "Organiser ID not found in token" });
            }

            int organiserId = int.Parse(organiserIdClaim.Value);
            
            var result = await _eventCreatorService.CreateEventAsync(eventDto, organiserId);
            
            if (result.Success)
            {
                return Ok(new { result.Message });
            }
            else
            {
                return BadRequest(new { result.Message });
            }
        }

        [HttpGet("get-events")]
        public async Task<IActionResult> GetEvents([FromQuery]GetEventRequestParameters parameters)
        {
            var paginationResponse = new PaginationResult<EventInfoDto>();
            try
            {
                paginationResponse = await _eventService.GetFiltratedEventsWithPagingAsync(parameters.Filter, parameters.Pagination.Page, parameters.Pagination.PageSize);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(paginationResponse);
        }
    }
}