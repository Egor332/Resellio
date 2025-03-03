using ResellioBackend.UserManagmentSystem.DTOs.Base;
using ResellioBackend.UserManagmentSystem.Factories.Abstractions;
using ResellioBackend.UserManagmentSystem.Models.Base;
using ResellioBackend.UserManagmentSystem.Repositories.Abstractions;
using ResellioBackend.UserManagmentSystem.Results;
using ResellioBackend.UserManagmentSystem.Services.Abstractions;

namespace ResellioBackend.UserManagmentSystem.Services.Implementations
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUsersRepository<UserBase> _userRepository;
        private readonly IUserFactory _userFactory;
        private readonly IPasswordService _passwordService;

        public RegistrationService(IUsersRepository<UserBase> userRepository, IUserFactory userFactory, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
            _passwordService = passwordService;
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

            return new RegistrationResult()
            {
                Success = true,
                Message = "Created successfully",
                Id = newUserId
            };
        }
    }
}
