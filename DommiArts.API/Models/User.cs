namespace DommiArts.API.Models
{

    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; } = null!; // usando null! para evitar o aviso de referência nula
        public string? Email { get; set; } = null!; // usando null! para evitar o aviso de referência nula
        public string? PasswordHash { get; set; } = null!; //usando null! para evitar o aviso de referência nula
        public string? PasswordSalt { get; set; } = null!; //usando null! para evitar o aviso de referência nula
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public string? Role { get; set; } = "User"; // e.g., "Admin", "User"

        // RefreshToken
        public String? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}