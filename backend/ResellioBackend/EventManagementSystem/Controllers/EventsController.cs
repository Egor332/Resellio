using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.UserManagementSystem.Statics;

namespace ResellioBackend.EventManagementSystem.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventCreatorService _eventCreatorService;
        
        public EventsController(IEventCreatorService eventCreatorService)
        {
            _eventCreatorService = eventCreatorService;
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
            
            var result = await _eventCreatorService.CreateEvent(eventDto, organiserId);
            
            if (result.Success)
            {
                return Ok(new { result.Message });
            }
            else
            {
                return BadRequest(new { result.Message });
            }
        }
    }
}