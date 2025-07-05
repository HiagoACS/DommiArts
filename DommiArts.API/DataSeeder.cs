using System.Security.Cryptography;
using System.Text;
using DommiArts.API.Data;
using DommiArts.API.Models;
using Microsoft.EntityFrameworkCore;
public class DataSeeder
{
    public static async Task SeedAdminUserAsync(DommiArtsDbContext context, IConfiguration configuration)
    {
        if (!await context.Users.AnyAsync(u => u.Role == "Admin"))
        {
            var password = configuration["AdminUser:Password"];
            if (string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Admin password not configured in appsettings or environment variables.");
            }

            CreatePasswordHash(password, out byte[] hash, out byte[] salt);

            var admin = new User
            {
                Username = "Administrator",
                Email = "admin@admin.com",
                PasswordHash = Convert.ToBase64String(hash),
                PasswordSalt = Convert.ToBase64String(salt),
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}
