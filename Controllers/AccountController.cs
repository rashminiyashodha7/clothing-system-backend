using GlamoraApi.Core.Interfaces;
using GlamoraApi.Data;
using GlamoraApi.DTOs;
using GlamoraApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;


namespace GlamoraApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;
        public AccountController(IAuthService authService, IUserRepository userRepository, ApplicationDbContext context)
        {
            _authService = authService;
            _userRepository = userRepository;
            _context = context;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return Ok(new { token = result.Token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
                return Unauthorized(new { errors = result.Errors });

            var userId = await _userRepository.GetUserByEmailAsync(dto.Email);

            // Return the token in the response
            return Ok(new
            {
                message = "Login successful",
                token = result.Token,
                userId = userId.UserId,
            });
        }

        [HttpGet("me")]
        [Authorize] 
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetUserByIdAsync(int.Parse(userId));

            return Ok(new
            {
                user.UserId,
                user.Name,
                user.Email
            });
        }

        [HttpGet("profile")]
        [Authorize] // Requires authentication
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userRepository.GetUserByIdAsync(int.Parse(userId));
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new
            {
                message = "Profile retrieved successfully",
                name = user.Name,
                email = user.Email,
                phone = user.Profile?.PhoneNumber,
                address = user.Profile?.Address
            });
        }

    }
}
