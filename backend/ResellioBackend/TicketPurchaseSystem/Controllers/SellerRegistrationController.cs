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
        private readonly string _organiserProfilePageUrl;

        public SellerRegistrationController(ISellerRegistrationService sellerRegistrationService, 
            IConfiguration configuration)
        {
            _sellerRegistrationService = sellerRegistrationService;
            _organiserProfilePageUrl = configuration["FrontEndLinks:OrganisersProfilePage"];
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
            try
            {
                string redirectStripeUrl = await _sellerRegistrationService.StartRegistrationAsync(userId, callbackUrl);

                return Ok(new { redirectStripeUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message, ex.StackTrace, ex.InnerException, ex.Source });
            }
        }

        [HttpGet("oauth-callback")]
        public async Task<IActionResult> OAuthCallback([FromQuery]string code, string state)
        {
            var result = await _sellerRegistrationService.CompleteRegistrationAsync(code, state);
            if (result)
            {
                return Redirect(_organiserProfilePageUrl);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
