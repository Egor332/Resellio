using System.Security.Claims;

namespace ResellioBackend.Services.Abstractions
{
    public interface ITokenGenerator
    {
        public string GenerateToken(List<Claim> claims, int expirationInMinuts, byte[] key, string issuer, string audience);
    }
}
