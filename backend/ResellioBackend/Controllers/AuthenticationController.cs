
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.DTOs;
using ResellioBackend.Services.Abstractions;

namespace ResellioBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginCredentialsDto credentials)
        {
            var result = await _authenticationService.LoginAsync(credentials);
            if (result.Success)
            {
                return Ok(new { result.Token, result.Message });
            }
            else
            {
                return BadRequest(new { result.Message });
            }
        }
    }
}
