using ResellioBackend.UserManagmentSystem.DTOs;
using ResellioBackend.UserManagmentSystem.Models.Base;
using ResellioBackend.UserManagmentSystem.Repositories.Abstractions;
using ResellioBackend.UserManagmentSystem.Results;
using ResellioBackend.UserManagmentSystem.Services.Abstractions;
using System.Text;

namespace ResellioBackend.UserManagmentSystem.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsersRepository<UserBase> _usersRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IUsersRepository<UserBase> usersRepository, IPasswordService passwordService, ITokenGenerator tokenGenerator,
            IConfiguration configuration)
        {
            _usersRepository = usersRepository;
            _passwordService = passwordService;
            _tokenGenerator = tokenGenerator;
            _configuration = configuration;
        }

        public async Task<UserAuthenticationResult> LoginAsync(LoginCredentialsDto credentials)
        {
            var email = credentials.Email;
            var password = credentials.Password;

            var user = await _usersRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return new UserAuthenticationResult()
                {
                    Success = false,
                    Message = "Wrong email or password was provided",
                };
            }

            var result = await ValidateAuthenticationAsync(email, password, user);

            if (!result.Success)
            {
                return result;
            }

            var token = GetSessionToken(user);
            result.Token = token;

            return result;
        }

        private async Task<UserAuthenticationResult> ValidateAuthenticationAsync(string email, string password, UserBase user)
        {
            var isPasswordCorrect = _passwordService.VerifyPassword(password, user.PasswordHash, user.Salt);
            if (!isPasswordCorrect)
            {
                return new UserAuthenticationResult()
                {
                    Success = false,
                    Message = "Wrong email or password was provided"
                };
            }
            var validationResult = user.ValidateAccount();
            if (!validationResult.Success)
            {
                return new UserAuthenticationResult()
                {
                    Success = false,
                    Message = validationResult.Message,
                };
            }
            return new UserAuthenticationResult()
            {
                Success = true,
                Message = "Success"
            };
        }

        private string GetSessionToken(UserBase user)
        {
            var claims = user.GetClaims();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtParameters:AuthenticationSecretKey"]);
            var issuer = _configuration["JwtParameters:Issuer"];
            var audience = _configuration["JwtParameters:AuthenticationAudience"];

            var token = _tokenGenerator.GenerateToken(claims, 60, key, issuer, audience);
            return token;
        }
    }
}
