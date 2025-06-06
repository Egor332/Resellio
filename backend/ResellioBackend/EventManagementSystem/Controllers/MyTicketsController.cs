using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Services.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Results;
using ResellioBackend.UserManagementSystem.Statics;

namespace ResellioBackend.EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyTicketsController : ControllerBase
    {
        private readonly IMyTicketService _myTicketService;

        public MyTicketsController(IMyTicketService myTicketService)
        {
            _myTicketService = myTicketService;
        }

        [Authorize]
        [HttpGet("qr-code")]
        public async Task<IActionResult> GetQRCode([FromQuery]Guid ticketId)
        {
            var userIdString = User.FindFirst(BearerTokenClaimsNames.Id);
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdString.Value);

            var result = await _myTicketService.GetQRCodeAsync(userId, ticketId);

            if (result.Success)
            {
                return Ok(new { result.QRCodeImage });
            }
            else
            {
                return BadRequest(new { result.Message });
            }
        }
        
    }
}
