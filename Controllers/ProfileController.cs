using GlamoraApi.Data;
using GlamoraApi.Data.Repositories;
using GlamoraApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GlamoraApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileService _profileService;
        private readonly IUserRepository _userRepository;

        public ProfileController(ProfileService profileService, IUserRepository userRepository)
        {
            _profileService = profileService;
            _userRepository = userRepository;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userRepository.GetUserByIdAsync(int.Parse(userId));
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.UserId,
                user.Name,
                user.Email,
                Profile = user.Profile
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] Profile profile)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var createdProfile = await _profileService.CreateProfileAsync(userId, profile);
            return CreatedAtAction(nameof(GetProfile), createdProfile);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] Profile profile)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var updatedProfile = await _profileService.UpdateProfileAsync(userId, profile);

            if (updatedProfile == null)
                return NotFound("Profile not found");

            return Ok(updatedProfile);
        }
    }
}
