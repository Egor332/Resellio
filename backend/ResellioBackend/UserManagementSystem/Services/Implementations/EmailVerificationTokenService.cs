using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using ResellioBackend.UserManagementSystem.Results;
using ResellioBackend.UserManagementSystem.Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ResellioBackend.UserManagementSystem.Services.Implementations
{
    public class EmailVerificationTokenService : IEmailVerificationTokenService
    {
        private readonly ITokenGenerator _tokenGenerator;
        private const string _idClaimName = "Id";
        private readonly byte[] _secretKey;

        public EmailVerificationTokenService(ITokenGenerator tokenGenerator, IConfiguration configuraiton)
        {
             _tokenGenerator = tokenGenerator;
            _secretKey = Encoding.UTF8.GetBytes(configuraiton["JwtParameters:EmailVerificationSecretKey"]);
        }

        public string GetEmailVerificationToken(int id)
        {
            var claims = new List<Claim>();
            Claim idClaim = new Claim(_idClaimName, id.ToString());
            claims.Add(idClaim);
            var token = _tokenGenerator.GenerateToken(claims, 60, _secretKey, "", "");
            return token;
        }

        public ValidateEmailVerificationTokenResult ValidateEmailVerificationToken(string token)
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
            // Microsoft.IdentityModel.Tokens.SecurityTokenMalformedException
            try
            {
                var claims = tokenHandler.ValidateToken(token, validationParameters, out _);
                var idClaim = claims.FindFirst(_idClaimName);
                if ((idClaim == null) || (string.IsNullOrEmpty(idClaim.Value)))
                {
                    return new ValidateEmailVerificationTokenResult
                    {
                        Success = false,
                        Message = "Incorrect token provided"
                    };
                }

                var id = int.Parse(idClaim.Value);

                return new ValidateEmailVerificationTokenResult
                {
                    Success = true,
                    UserId = id,
                };
            }
            catch (SecurityTokenExpiredException)
            {
                return new ValidateEmailVerificationTokenResult
                {
                    Success = false,
                    Message = "Token has expired."
                };
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                return new ValidateEmailVerificationTokenResult
                {
                    Success = false,
                    Message = "Invalid token signature."
                };
            }
            catch (Exception ex)
            {
                return new ValidateEmailVerificationTokenResult
                {
                    Success = false,
                    Message = $"Invalid token: {ex.Message}"
                };
            }
            
        }
    }
}
