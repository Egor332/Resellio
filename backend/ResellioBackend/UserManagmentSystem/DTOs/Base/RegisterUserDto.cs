namespace ResellioBackend.UserManagmentSystem.DTOs.Base
{
    public abstract class RegisterUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
