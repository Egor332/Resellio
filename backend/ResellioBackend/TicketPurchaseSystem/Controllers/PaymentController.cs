using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.UserManagementSystem.Statics;
using System.Runtime.CompilerServices;

namespace ResellioBackend.TicketPurchaseSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ICheckoutSessionCreatorService _checkoutSessionCreatorService;
        private readonly string _publishableKey;

        public PaymentController(ICheckoutSessionCreatorService checkoutSessionCreatorService, IConfiguration configuration) 
        {
            _checkoutSessionCreatorService = checkoutSessionCreatorService;
            _publishableKey = configuration["Stripe:PublishableKey"]!;
        }

        [Authorize(Policy = AuthorizationPolicies.CustomerPolicy)]
        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession()
        {
            var userIdClaim = User.FindFirst(BearerTokenClaimsNames.Id);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdClaim.Value);
            var sessionCreationResult = await _checkoutSessionCreatorService.CreateCheckoutSessionAsync(userId);
            if (sessionCreationResult.Success)
            {
                return Ok(new { PyblishableKey = _publishableKey, SessionId = sessionCreationResult.CreatedSession.Id });
            }
            else
            {
                return BadRequest(sessionCreationResult.Message);
            }
        }

        [NonAction]
        [HttpPost("payment-webhook")]
        public async Task<IActionResult> PaymentWebhook()
        {
            return Ok();
        }
    }
}
