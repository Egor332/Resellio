using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Users;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;

namespace ResellioBackend.Utilities
{
#if DEBUG
    [Route("api/[controller]")]
    [ApiController]
    public class DevelopmentController : ControllerBase
    {
        private readonly IUsersRepository<UserBase> _usersRepository;
        private readonly IUsersRepository<Organiser> _organiserRepository;

        public DevelopmentController(IUsersRepository<UserBase> usersRepository, IUsersRepository<Organiser> organiserRepository)
        {
            _usersRepository = usersRepository;
            _organiserRepository = organiserRepository;
        }

        [HttpGet("set-email-valid")]
        public async Task<IActionResult> SetEmailValid([FromQuery]string email) 
        {
            try
            {
                var user = await _usersRepository.GetByEmailAsync(email);
                user.IsActive = true;
                await _usersRepository.UpdateAsync(user);
                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest();
            }
        }

        [HttpGet("set-organiser-valid")]
        public async Task<IActionResult> SetOrganiserValid([FromQuery]string email)
        {
            try
            {
                var user = await _organiserRepository.GetByEmailAsync(email);
                user.IsVerified = true;
                await _organiserRepository.UpdateAsync(user);
                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest();
            }
        }
    }
#endif
}
