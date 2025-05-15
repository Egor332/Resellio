using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.UserManagementSystem.Statics;

namespace ResellioBackend.TicketPurchaseSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerRegistrationController : ControllerBase
    {
        private readonly ISellerRegistrationService _sellerRegistrationService;

        public SellerRegistrationController(ISellerRegistrationService sellerRegistrationService)
        {
            _sellerRegistrationService = sellerRegistrationService;
        }

        [Authorize]
        [HttpGet("connect")]
        public async Task<IActionResult> Connect()
        {
            var userIdClaim = User.FindFirst(BearerTokenClaimsNames.Id);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdClaim.Value);
            var request = HttpContext.Request;
            var hostInfo = $"{request.Scheme}://{request.Host.Value}";
            var callbackUrl = hostInfo + "/api/SellerRegistration/oauth-callback";
            string redirectStripeUrl = await _sellerRegistrationService.StartRegistrationAsync(userId, callbackUrl);

            return Redirect(redirectStripeUrl);
        }

        [HttpGet("oauth-callback")]
        public async Task<IActionResult> OAuthCallback([FromQuery]string code, string state)
        {
            var result = await _sellerRegistrationService.CompleteRegistrationAsync(code, state);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
