using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.DTOs.Users;
using ResellioBackend.Factories.Abstractions;

namespace ResellioBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUserFactory _userFactory;

        public CustomersController(IUserFactory userFactory) 
        {
            _userFactory = userFactory;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterCustomerDto registrationInfo) 
        {
            
            return Ok();
        }
    }
}
