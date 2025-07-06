using DommiArts.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DommiArts.API.Data;
using AutoMapper;
using DommiArts.API.DTOs.User;

namespace DommiArts.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DommiArtsDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthController(DommiArtsDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }

        private string CreateToken(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }
            if (user.Username == null)
            {
                throw new ArgumentNullException(nameof(user.Username), "Username cannot be null");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não está configurada"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role ?? "User")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],      
                Audience = _configuration["Jwt:Audience"], 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDTO userCreateDto)
        {

            if (userCreateDto == null)
            {
                return BadRequest("User data is required.");
            }
            if (string.IsNullOrEmpty(userCreateDto.Username) || string.IsNullOrEmpty(userCreateDto.Email) || string.IsNullOrEmpty(userCreateDto.Password))
            {
                return BadRequest("Username, email, and password are required.");
            }
            if (await _context.Users.AnyAsync(u => u.Username == userCreateDto.Username))
            {
                return BadRequest("Username already exists.");
            }
            if (await _context.Users.AnyAsync(u => u.Email == userCreateDto.Email))
            {
                return BadRequest("Email already exists.");
            }

            CreatePasswordHash(userCreateDto.Password, out byte[] passwordHash, out byte[] passwordSalt); // Criando o hash e salt da senha

            //Criando o usuário a partir do DTO e Convertendo o hash e salt da senha para Base64
            var user = _mapper.Map<User>(userCreateDto);
            user.PasswordHash = Convert.ToBase64String(passwordHash);
            user.PasswordSalt = Convert.ToBase64String(passwordSalt);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "User registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDto)
        {
            if (userLoginDto == null)
            {
                return BadRequest("User data is required.");
            }
            if (userLoginDto.Password == null)
            {
                return BadRequest("User Password is required");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userLoginDto.Username);

            if (user == null || user.PasswordHash == null || user.PasswordSalt == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var passwordHash = Convert.FromBase64String(user.PasswordHash);
            var passwordSalt = Convert.FromBase64String(user.PasswordSalt);

            if (!VerifyPasswordHash(userLoginDto.Password, passwordHash, passwordSalt))
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = CreateToken(user);
            return Ok(new { token });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO userUpdateDto)
        {
            if (userUpdateDto == null)
            {
                return BadRequest("User data is required.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Atualiza apenas os campos permitidos
            if (!string.IsNullOrWhiteSpace(userUpdateDto.Username))
            {
                user.Username = userUpdateDto.Username;
            }
            if (!string.IsNullOrWhiteSpace(userUpdateDto.Email))
            {
                user.Email = userUpdateDto.Email;
            }
            if (!string.IsNullOrWhiteSpace(userUpdateDto.Password))
            {
                CreatePasswordHash(userUpdateDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = Convert.ToBase64String(passwordHash);
                user.PasswordSalt = Convert.ToBase64String(passwordSalt);
            }
            user.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }



    }
}
        