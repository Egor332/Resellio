using ResellioBackend.UserManagmentSystem.DTOs.Base;

namespace ResellioBackend.UserManagmentSystem.DTOs.Users
{
    public class RegisterOrganiserDto : RegisterUserDto
    {
        public string OrganiserName { get; set; }
    }
}
