using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.UserManagementSystem.DTOs;
using ResellioBackend.UserManagementSystem.Services.Abstractions;

namespace ResellioBackend.UserManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private readonly IPasswordResetService _passwordResetService;
        private readonly IPasswordResetTokenService _passwordResetTokenService;
        private readonly IConfiguration _configuration;

        public PasswordResetController(IPasswordResetService passwordResetService,  IPasswordResetTokenService passwordResetTokenService, IConfiguration configuration)
        {
            _passwordResetService = passwordResetService;
            _passwordResetTokenService = passwordResetTokenService;
            _configuration = configuration;
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody]RequestPasswordResetDto dto)
        {
            var result = await _passwordResetService.RequestPasswordResetAsync(dto.Email);
            if (result.Success)
            {
                return Ok(new { result.Message });
            }
            else
            {
                return BadRequest(new { result.Message });
            }
        }

        [HttpGet("redirect-to-form")]
        public IActionResult RedirectToForm([FromQuery]string token)
        {
            var result = _passwordResetTokenService.VerifyPasswordResetToken(token);
            if (result.Success)
            {
                var linkWithEmptyParameter = _configuration["FrontEndLinks:PasswordResetFormLinkWithEmptyTokenParameter"];
                var linkWithToken = linkWithEmptyParameter + token;
                return Redirect(linkWithToken);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordDto dto)
        {
            var result = await _passwordResetService.ChangePasswordAsync(dto);
            if (result.Success)
            {
                return Ok(new { result.Message });
            }
            else
            {
                return NotFound(new { result.Message });
            }
        }
    }
}
