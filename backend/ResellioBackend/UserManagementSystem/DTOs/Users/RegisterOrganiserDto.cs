using ResellioBackend.UserManagementSystem.DTOs.Base;

namespace ResellioBackend.UserManagementSystem.DTOs.Users
{
    public class RegisterOrganiserDto : RegisterUserDto
    {
        public string OrganiserName { get; set; }
    }
}
