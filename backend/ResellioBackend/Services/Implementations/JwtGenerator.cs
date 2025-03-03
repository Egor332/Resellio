using Microsoft.IdentityModel.Tokens;
using ResellioBackend.Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ResellioBackend.Services.Implementations
{
    public class JwtGenerator : ITokenGenerator
    {
        public string GenerateToken(List<Claim> claims, int expirationInMinutes, byte[] key, string issuer, string audience)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
