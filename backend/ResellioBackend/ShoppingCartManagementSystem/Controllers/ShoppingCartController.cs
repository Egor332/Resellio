using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.ShoppingCartManagementSystem.DTOs;
using ResellioBackend.ShoppingCartManagementSystem.Services.Abstractions;
using ResellioBackend.UserManagementSystem.Statics;

namespace ResellioBackend.ShoppingCartManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ITicketLockerService _ticketLockerService;
        private readonly ITicketUnlockerService _ticketUnlockerService;

        public ShoppingCartController(ITicketLockerService ticketLockerService,  ITicketUnlockerService ticketUnlockerService)
        {
            _ticketLockerService = ticketLockerService;
            _ticketUnlockerService = ticketUnlockerService;
        }

        [Authorize(Policy = AuthorizationPolicies.CustomerPolicy)]
        [HttpPost("LockTicket")]
        public async Task<IActionResult> LockTicket([FromBody]LockTicketDto dto)
        {
            var userIdString = User.FindFirst(BearerTokenClaimsNames.Id).ToString();
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdString);
            var result = await _ticketLockerService.LockTicketAsync(userId, dto.TicketId);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
