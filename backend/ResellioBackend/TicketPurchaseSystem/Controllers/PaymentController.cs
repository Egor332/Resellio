using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.UserManagementSystem.Statics;
using System.Runtime.CompilerServices;

namespace ResellioBackend.TicketPurchaseSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [Authorize(Policy = AuthorizationPolicies.CustomerPolicy)]
        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession()
        {
            return Ok();
        }

        [NonAction]
        [HttpPost("payment-webhook")]
        public async Task<IActionResult> PaymentWebhook()
        {
            return Ok();
        }
    }
}
