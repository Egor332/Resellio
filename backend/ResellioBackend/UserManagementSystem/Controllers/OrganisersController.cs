using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.UserManagementSystem.DTOs.Users;
using ResellioBackend.UserManagementSystem.Services.Abstractions;

namespace ResellioBackend.UserManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganisersController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public OrganisersController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterOrganiserDto registrationInfo)
        {
            var result = await _registrationService.RegisterUserAsync(registrationInfo);
            // TODO if not microservice then we should send an email
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
