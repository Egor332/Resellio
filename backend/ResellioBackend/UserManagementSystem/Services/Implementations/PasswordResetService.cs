using ResellioBackend.Kafka;
using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.DTOs;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Emails;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Services.Abstractions;

namespace ResellioBackend.UserManagementSystem.Services.Implementations
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUsersRepository<UserBase> _usersRepository;
        private readonly IPasswordResetTokenService _passwordResetTokenService;
        private readonly IPasswordResetTokenRepository _tokenRepository;
        private readonly IPasswordService _passwordService;
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;

        public PasswordResetService(IUsersRepository<UserBase> usersRepository,  IPasswordResetTokenService passwordResetTokenService, IPasswordResetTokenRepository tokenRepository,
           IPasswordService passwordService, IKafkaProducerService kafkaProducerService, IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            _usersRepository = usersRepository;
            _passwordResetTokenService = passwordResetTokenService;
            _tokenRepository = tokenRepository;
            _passwordService = passwordService;
            _kafkaProducerService = kafkaProducerService;
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
        }

        public async Task<ResultBase> RequestPasswordResetAsync(string email)
        {
            var user = await _usersRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = "If this user exist, we sent an email"
                };
            }
            var token = _passwordResetTokenService.CreateTokenWithDatabaseRecordAsync(user.UserId);

            var userEmail = user.Email;
            var subject = "Resellio password reset";
            var confirmationLink = _linkGenerator.GetUriByAction(
                _httpContextAccessor.HttpContext,
                action: "RedirectToForm",
                controller: "PasswordReset",
                values: new { token },
                scheme: _httpContextAccessor.HttpContext.Request.Scheme
            );
            var htmlContent = $"Click this link to reset your password: {confirmationLink}";

            var resetPasswordEmail = new EmailForSendGrid()
            {
                Email = userEmail,
                Subject = subject,
                PlainTextContent = "",
                HtmlContent = htmlContent
            };
            await _kafkaProducerService.SendMessageAsync(resetPasswordEmail);

            return new ResultBase()
            {
                Success = true,
                Message = "If this user exist, we sent an email"
            };
        }

        public async Task<ResultBase> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            var newPassword = changePasswordDto.NewPassword;
            var token = changePasswordDto.Token;
            var tokenVerificationResult = _passwordResetTokenService.VerifyPasswordResetToken(token);
            if (!tokenVerificationResult.Success)
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = tokenVerificationResult.Message
                };
            }
            var tokenId = tokenVerificationResult.TokenId;
            var tokenInfo = await _tokenRepository.GetByIdAsync(tokenId);
            if (tokenInfo == null) 
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = "Unregistered token"
                };
            }
            await _tokenRepository.DeleteTokenAsync(tokenInfo);

            var userId = tokenVerificationResult.UserId;
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = "Invalid token"
                };
            }

            (string passwordHash, string salt) = _passwordService.HashPassword(newPassword);
            user.ChangePassword(passwordHash, salt);

            await _usersRepository.UpdateAsync(user);

            return new ResultBase()
            {
                Success = true,
                Message = "Password had been changed"
            };
        }
    }
}
