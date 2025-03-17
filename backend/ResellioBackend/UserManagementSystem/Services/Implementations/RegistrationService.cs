using ResellioBackend.UserManagementSystem.DTOs.Base;
using ResellioBackend.UserManagementSystem.Factories.Abstractions;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Results;
using ResellioBackend.UserManagementSystem.Services.Abstractions;

namespace ResellioBackend.UserManagementSystem.Services.Implementations
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUsersRepository<UserBase> _userRepository;
        private readonly IUserFactory _userFactory;
        private readonly IPasswordService _passwordService;
        private readonly IEmailVerificationService _emailVerificationService;

        public RegistrationService(IUsersRepository<UserBase> userRepository, IUserFactory userFactory, IPasswordService passwordService, IEmailVerificationService emailVerificationService)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
            _passwordService = passwordService;
            _emailVerificationService = emailVerificationService;
        }

        public async Task<RegistrationResult> RegisterUserAsync(RegisterUserDto dto)
        {
            var userWithThisEmail = await _userRepository.GetByEmailAsync(dto.Email);
            if (userWithThisEmail != null)
            {
                return new RegistrationResult
                {
                    Success = false,
                    Message = "User with such email already exists"
                };
            }

            var password = dto.Password;
            var newUser = _userFactory.CreateNewUserWithoutPassword(dto);
            (string passwordHash, string salt) = _passwordService.HashPassword(password);
            newUser.PasswordHash = passwordHash;
            newUser.Salt = salt;

            await _userRepository.AddAsync(newUser);
            int newUserId = newUser.UserId;

            await _emailVerificationService.CreateAndSendVerificationEmailAsync(newUser);

            return new RegistrationResult()
            {
                Success = true,
                Message = "Created successfully",
                Id = newUserId
            };
        }
    }
}
