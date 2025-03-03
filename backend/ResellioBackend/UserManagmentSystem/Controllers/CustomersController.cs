using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.UserManagmentSystem.Factories.Abstractions;
using ResellioBackend.UserManagmentSystem.DTOs.Users;
using ResellioBackend.UserManagmentSystem.Services.Abstractions;

namespace ResellioBackend.UserManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public CustomersController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerDto registrationInfo)
        {
            var result = await _registrationService.RegisterUserAsync(registrationInfo);
            if (result.Success)
            {
                return Ok(new { result.Id, result.Message });
            }
            else
            {
                return BadRequest(new { result.Message });
            }
        }
    }
}
