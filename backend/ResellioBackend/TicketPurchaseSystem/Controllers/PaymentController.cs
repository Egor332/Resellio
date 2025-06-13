using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.TicketPurchaseSystem.DTOs;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.UserManagementSystem.Statics;
using Stripe;

namespace ResellioBackend.TicketPurchaseSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ICheckoutSessionCreatorService _checkoutSessionCreatorService;
        private readonly string _publishableKey;
        private readonly string _webhookSecretKey;
        private readonly ICheckoutEventProcessor _checkoutEventProcessor;

        public PaymentController(ICheckoutSessionCreatorService checkoutSessionCreatorService, IConfiguration configuration,
            ICheckoutEventProcessor checkoutEventProcessor) 
        {
            _checkoutSessionCreatorService = checkoutSessionCreatorService;
            _publishableKey = configuration["Stripe:PublishableKey"]!;
            _webhookSecretKey = configuration["Stripe:WebhookSecretKey"]!;
            _checkoutEventProcessor = checkoutEventProcessor;
        }

        [Authorize(Policy = AuthorizationPolicies.CustomerPolicy)]
        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionDto dto)
        {
            var userIdClaim = User.FindFirst(BearerTokenClaimsNames.Id);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdClaim.Value);
            try
            {
                var sessionCreationResult = await _checkoutSessionCreatorService.CreateCheckoutSessionAsync(userId, dto.SellerId);
                if (sessionCreationResult.Success)
                {
                    return Ok(new { PublishableKey = _publishableKey, SessionId = sessionCreationResult.CreatedSession.Id });
                }
                else
                {
                    return BadRequest(sessionCreationResult.Message);
                }
            }
            catch (Exception ex) 
            {
                return BadRequest(new { ex.Message, ex.StackTrace, ex.InnerException, ex.Source });
            }
        }

        [HttpPost("payment-webhook")]
        public async Task<IActionResult> PaymentWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            Stripe.Event stripeEvent; // with namespace to escape collisions with our Event model

            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _webhookSecretKey);
                var result = await _checkoutEventProcessor.ProcessCheckoutEventAsync(stripeEvent);
                return Ok(result.Message);
            }
            catch (StripeException ex) 
            {
                Console.WriteLine($"Webhook error: {ex.Message}");
                return BadRequest(ex.Message);
            }

            
        }
    }
}
