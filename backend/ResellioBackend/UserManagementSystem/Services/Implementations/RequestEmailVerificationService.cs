using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Services.Abstractions;

namespace ResellioBackend.UserManagementSystem.Services.Implementations
{
    public class RequestEmailVerificationService : IRequestEmailVerificationService
    {
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IUsersRepository<UserBase> _usersRepository;

        public RequestEmailVerificationService(IEmailVerificationService emailVerificationService, IUsersRepository<UserBase> usersRepository)
        {
            _emailVerificationService = emailVerificationService;
            _usersRepository = usersRepository;
        }

        public async Task<ResultBase> ResentEmailVerificationMessageAsync(string email)
        {
            var user = await _usersRepository.GetByEmailAsync(email);
            if ((user == null) || (user.IsActive == false))
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = "Email have already been confirmed or does not registered"
                };
            }
            await _emailVerificationService.CreateAndSendVerificationEmailAsync(user);
            return new ResultBase()
            {
                Success = true,
                Message = "Verification message had been sent"
            };
        }
    }
}
