using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.TicketPurchaseSystem.DTOs;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.UserManagementSystem.Statics;

namespace ResellioBackend.TicketPurchaseSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ITicketLockerService _ticketLockerService;
        private readonly ITicketUnlockerService _ticketUnlockerService;
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(ITicketLockerService ticketLockerService, ITicketUnlockerService ticketUnlockerService, IShoppingCartService shoppingCartService)
        {
            _ticketLockerService = ticketLockerService;
            _ticketUnlockerService = ticketUnlockerService;
            _shoppingCartService = shoppingCartService;
        }

        [Authorize(Policy = AuthorizationPolicies.CustomerPolicy)]
        [HttpPost("lock-ticket")]
        public async Task<IActionResult> LockTicket([FromBody] TicketDto dto)
        {
            var userIdString = User.FindFirst(BearerTokenClaimsNames.Id);
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdString.Value);
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

        [Authorize(Policy = AuthorizationPolicies.CustomerPolicy)]
        [HttpPost("unlock-ticket")]
        public async Task<IActionResult> UnlockTicket([FromBody] TicketDto dto)
        {
            var userIdString = User.FindFirst(BearerTokenClaimsNames.Id);
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdString.Value);
            var result = await _ticketUnlockerService.UnlockTicketAsync(userId, dto.TicketId);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [Authorize(Policy = AuthorizationPolicies.CustomerPolicy)]
        [HttpGet("cart-info")]
        public async Task<IActionResult> CartInfo()
        {
            var userIdString = User.FindFirst(BearerTokenClaimsNames.Id);
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdString.Value);
            var result = await _shoppingCartService.GetShoppingCartInfoAsync(userId);
            return Ok(result);
        }
    }
}
