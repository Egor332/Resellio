
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.UserManagementSystem.DTOs;
using ResellioBackend.UserManagementSystem.Services.Abstractions;

namespace ResellioBackend.UserManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfirmEmailService _confirmEmailService;
        private readonly IRequestEmailVerificationService _requestEmailVerificationService;
        private readonly string _loginFrontEndUrl;
        public AuthenticationController(IAuthenticationService authenticationService, IConfirmEmailService confirmEmailService,  IRequestEmailVerificationService requestEmailVerificationService,
            IConfiguration configuration)
        {
            _authenticationService = authenticationService;
            _confirmEmailService = confirmEmailService;
            _requestEmailVerificationService = requestEmailVerificationService;
            _loginFrontEndUrl = configuration["FrontEndLinks:Login"];
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentialsDto credentials)
        {
            try
            {
                var result = await _authenticationService.LoginAsync(credentials);
                if (result.Success)
                {
                    return Ok(new { result.Token, result.UserRole, result.Message });
                }
                else
                {
                    return BadRequest(new { result.Message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex });
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery]string token)
        {
            var result = await _confirmEmailService.ConfirmEmailAsync(token);
            if (result.Success)
            {
                return Redirect(_loginFrontEndUrl);
            }
            else
            {
                return BadRequest(new { result.Message });
            }            
        }

        [HttpPost("resent-verification-email")]
        public async Task<IActionResult> ResentVerificationEmail([FromBody]EmailDto emailDto)
        {
            var result = await _requestEmailVerificationService.ResentEmailVerificationMessageAsync(emailDto.Email);
            if (result.Success)
            {
                return Ok(new { result.Message });
            }
            else
            {
                return BadRequest(new { result.Message }); 
            }
        }
    }
}
