using Microsoft.AspNetCore.Http.HttpResults;
using ResellioBackend.Kafka;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Emails;
using ResellioBackend.UserManagementSystem.Services.Abstractions;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ResellioBackend.UserManagementSystem.Services.Implementations
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly IEmailVerificationTokenService _tokenService;
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;

        public EmailVerificationService(IEmailVerificationTokenService tokenService, IKafkaProducerService kafkaProducerService,
            IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            _tokenService = tokenService;
            _kafkaProducerService = kafkaProducerService;
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;

        }

        public async Task CreateAndSendVerificationEmailAsync(UserBase user)
        {
            var userEmail = user.Email;
            var subject = "Verify your email";
            var token = _tokenService.GetEmailVerificationToken(user.UserId);
            var confirmationLink = _linkGenerator.GetUriByAction(
                _httpContextAccessor.HttpContext,
                action: "ConfirmEmail",
                controller: "Authentication",
                values: new { token },
                scheme: _httpContextAccessor.HttpContext.Request.Scheme
            );
            var htmlContent = $"Verify your email by clicking this link: {confirmationLink}";

            var verificationEmail = new EmailForSendGrid()
            {
                Email = userEmail,
                Subject = subject,
                PlainTextContent = "",
                HtmlContent = htmlContent
            };
            await _kafkaProducerService.SendMessageAsync(verificationEmail);
        }
    }
}
