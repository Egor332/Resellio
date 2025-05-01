namespace ResellioBackend.UserManagementSystem.DTOs.Base
{
    public class UserInfoDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? OrganiserName { get; set; }
        
    }
}
