using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.DTOs;

namespace ResellioBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public AuthenticationController()
        {
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginCredentialsDto credentials)
        {
            return Ok();
        }
    }
}
