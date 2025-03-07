using Microsoft.IdentityModel.Tokens;
using ResellioBackend.UserManagementSystem.Models.Tokens;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Results;
using ResellioBackend.UserManagementSystem.Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace ResellioBackend.UserManagementSystem.Services.Implementations
{
    public class PasswordResetTokenService : IPasswordResetTokenService
    {
        private readonly IPasswordResetTokenRepository _tokensRepository;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly byte[] _secretKey;
        public const string IdClaimName = "Id";
        public const string UserIdClaimName = "UserId";

        public PasswordResetTokenService(IPasswordResetTokenRepository tokensRepository, IConfiguration configuration, ITokenGenerator tokenGenerator)
        {
            _tokensRepository = tokensRepository;
            _secretKey = Encoding.UTF8.GetBytes(configuration["JwtParameters:PasswordResetSecretKey"]);
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> CreateTokenWithDatabaseRecordAsync(int userId)
        {
            var usersTokenList = await _tokensRepository.GetUserTokensAsync(userId);
            foreach (var token in usersTokenList)
            {
                await _tokensRepository.DeleteTokenAsync(token);
            }

            var newTokenGuid = Guid.NewGuid();
            List<Claim> claims = new List<Claim>();
            Claim idClaim = new Claim(IdClaimName, newTokenGuid.ToString());
            Claim userIdCliam = new Claim(UserIdClaimName, userId.ToString());
            claims.Add(idClaim);
            claims.Add(userIdCliam);
            var newToken = _tokenGenerator.GenerateToken(claims, 10, _secretKey, "", "");

            var newTokenInfo = new PasswordResetTokenInfo()
            {
                Id = newTokenGuid,
                CreationDate = DateTime.Now,
                ExpiryDate = (DateTime.Now).AddMinutes(10),
                OwnerId = userId,
            };

            await _tokensRepository.AddTokenAsync(newTokenInfo);

            return newToken;
        }

        public VerifyResetPasswordTokenResult VerifyPasswordResetToken(string token)
        {

            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_secretKey)
            };

            try
            {
                var claims = tokenHandler.ValidateToken(token, validationParameters, out _);
                var tokenIdClaim = claims.FindFirst(IdClaimName);
                var userIdClaim = claims.FindFirst(UserIdClaimName);
                if ((tokenIdClaim == null) || (string.IsNullOrEmpty(tokenIdClaim.Value)) ||
                    (userIdClaim == null) || (string.IsNullOrEmpty(userIdClaim.Value)))
                {
                    return new VerifyResetPasswordTokenResult
                    {
                        Success = false,
                        Message = "Incorrect token provided"
                    };
                }

                var tokenId = Guid.Parse(tokenIdClaim.Value);
                var userId = int.Parse(userIdClaim.Value);

                return new VerifyResetPasswordTokenResult
                {
                    Success = true,
                    UserId = userId,
                    TokenId = tokenId,
                };
            }
            catch (SecurityTokenExpiredException)
            {
                return new VerifyResetPasswordTokenResult
                {
                    Success = false,
                    Message = "Token has expired."
                };
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                return new VerifyResetPasswordTokenResult
                {
                    Success = false,
                    Message = "Invalid token signature."
                };
            }
            catch (Exception ex)
            {
                return new VerifyResetPasswordTokenResult
                {
                    Success = false,
                    Message = $"Invalid token: {ex.Message}"
                };
            }
        }
    }
}
