using System.Security.Claims;

namespace ResellioBackend.UserManagementSystem.Services.Abstractions
{
    public interface ITokenGenerator
    {
        public string GenerateToken(List<Claim> claims, int expirationInMinutes, byte[] key, string issuer, string audience);
    }
}
