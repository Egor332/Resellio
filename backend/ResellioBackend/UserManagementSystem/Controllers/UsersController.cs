using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Services.Abstractions;
using ResellioBackend.UserManagementSystem.Statics;

namespace ResellioBackend.UserManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("user-info")]
        public async Task<IActionResult> UserInfo()
        {
            var userIdString = User.FindFirst(BearerTokenClaimsNames.Id);
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdString.Value);

            var result = await _userService.GetUserInfoAsync(userId);
            if (result.Success)
            {
                return Ok(result.userInfo);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
