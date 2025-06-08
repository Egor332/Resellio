using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.UserManagementSystem.DTOs;
using ResellioBackend.UserManagementSystem.Services.Abstractions;

namespace ResellioBackend.UserManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {
        private readonly IResetPasswordService _passwordResetService;
        private readonly IPasswordResetTokenService _passwordResetTokenService;
        private readonly IConfiguration _configuration;

        public ResetPasswordController(IResetPasswordService passwordResetService,  IPasswordResetTokenService passwordResetTokenService, IConfiguration configuration)
        {
            _passwordResetService = passwordResetService;
            _passwordResetTokenService = passwordResetTokenService;
            _configuration = configuration;
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestResetPassword([FromBody]RequestResetPasswordDto dto)
        {
            var result = await _passwordResetService.RequestResetPasswordAsync(dto.Email);
            return Ok(new { result.Message });
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

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDto dto)
        {
            var result = await _passwordResetService.ResetPasswordAsync(dto);
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
