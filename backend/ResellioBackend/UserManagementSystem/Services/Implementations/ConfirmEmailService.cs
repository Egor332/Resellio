using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Services.Abstractions;
using System.Runtime.CompilerServices;

namespace ResellioBackend.UserManagementSystem.Services.Implementations
{
    public class ConfirmEmailService : IConfirmEmailService
    {
        private readonly IEmailVerificationTokenService _emailVerificationTokenService;
        private readonly IUsersRepository<UserBase> _usersRepository;

        public ConfirmEmailService(IEmailVerificationTokenService emailVerificationTokenService, IUsersRepository<UserBase> usersRepository)
        {
            _emailVerificationTokenService = emailVerificationTokenService;
            _usersRepository = usersRepository;
        }

        public async Task<ResultBase> ConfirmEmailAsync(string token)
        {
            var validationResult = _emailVerificationTokenService.ValidateEmailVerificationToken(token);
            if (!validationResult.Success) 
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = validationResult.Message
                };
            }
            var user = await _usersRepository.GetByIdAsync(validationResult.UserId);
            if (user == null)
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = "User doesn't exist"
                };
            }
            user.IsActive = true;
            await _usersRepository.UpdateAsync(user);
            return new ResultBase() 
            {
                Success = true,
                Message = "Your email was confirmed"
            };
        }
    }
}
