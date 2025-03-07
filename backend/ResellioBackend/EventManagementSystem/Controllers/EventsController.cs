using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.DTOs;

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

        [HttpPost("create")]
        public async Task<IActionResult> CreateEvent([FromBody] EventDto eventDto)
        {
            var organiserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
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
    }
}