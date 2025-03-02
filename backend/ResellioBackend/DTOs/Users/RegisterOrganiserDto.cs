using ResellioBackend.DTOs.Base;

namespace ResellioBackend.DTOs.Users
{
    public class RegisterOrganiserDto : RegisterUserDto
    {
        public string OrganiserName { get; set; }
    }
}
