using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        [HttpGet("check")]
        public async Task<IActionResult> Check()
        {
            return Ok(new { Message = "Check" });
        }
    }
}
