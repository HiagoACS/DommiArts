namespace DommiArts.API.DTOs.User
{
    public class UserAdminCreateDTO
    {
        public string? Username { get; set; } = null!;
        public string? Email { get; set; } = null;
        public string? Password { get; set; } = null!;

        public string Role { get; set; } = "Admin";
    }
}