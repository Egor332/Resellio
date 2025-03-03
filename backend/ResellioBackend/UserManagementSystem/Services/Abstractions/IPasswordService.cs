namespace ResellioBackend.UserManagementSystem.Services.Abstractions
{
    public interface IPasswordService
    {
        public (string hash, string salt) HashPassword(string password);
        public bool VerifyPassword(string password, string passwordHash, string salt);
    }
}
