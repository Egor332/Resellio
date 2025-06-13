using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly ILogger<IndexController> _logger;
        public IndexController(ILogger<IndexController> logger)
        {
            _logger = logger;
        }

        [HttpGet("check")]
        public async Task<IActionResult> Check()
        {
            _logger.LogInformation("Check log");
            return Ok(new { Message = "Check" });
        }
    }
}
