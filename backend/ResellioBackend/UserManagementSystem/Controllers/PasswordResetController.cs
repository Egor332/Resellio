using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ResellioBackend.UserManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        [HttpGet("redirect-to-form")]
        public async Task<IActionResult> RedirectToForm([FromQuery]string token)
        {
            return Ok();
        }
    }
}
