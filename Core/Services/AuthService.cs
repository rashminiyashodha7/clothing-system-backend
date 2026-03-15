// Core/Services/AuthService.cs
using GlamoraApi.Core.Interfaces;
using GlamoraApi.DTOs;
using GlamoraApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }

    public async Task<AuthResult> RegisterAsync(RegisterDto dto)
    {
        // Validate email format
        if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains("@"))
        {
            return new AuthResult { Errors = new[] { "Invalid email format" } };
        }

        User existingUser;
        try
        {
            existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
        }
        catch (KeyNotFoundException)
        {
            existingUser = null; 

        }

        if (existingUser != null)
        {
            return new AuthResult { Errors = new[] { "User already exists" } };
        }

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash),  //password hash
     
        };

        await _userRepository.AddUserAsync(user);

        var token = GenerateJwtToken(user);
        return new AuthResult { Success = true, Token = token, UserId = user.UserId };
    }

    public async Task<AuthResult> LoginAsync(LoginDto dto)
    {
        User user = null;

        try
        {
            user = await _userRepository.GetUserByEmailAsync(dto.Email);
        }
        catch (KeyNotFoundException)
        {

        }

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.PasswordHash, user.PasswordHash))
        {
            return new AuthResult { Errors = new[] { "Invalid credentials" } };
        }

        var token = GenerateJwtToken(user);
        return new AuthResult { Success = true, Token = token };
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Secret"]);

        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(_config.GetValue<double>("Jwt:ExpiryInMinutes"));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("name", user.Name)
        }),
            NotBefore = now,
            IssuedAt = now,
            Expires = expires > now ? expires : now.AddSeconds(1), 
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }


}
