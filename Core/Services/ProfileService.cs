
using GlamoraApi.Data;
using GlamoraApi.Models;
using Microsoft.EntityFrameworkCore;

public class ProfileService
{
    private readonly ApplicationDbContext _context;

    public ProfileService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Profile> CreateProfileAsync(int userId, Profile profile)
    {
        profile.UserId = userId;
        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync();
        return profile;
    }

    public async Task<Profile> GetUserProfileAsync(int userId)
    {
        return await _context.Profiles
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<Profile> UpdateProfileAsync(int userId, Profile updatedProfile)
    {
        var profile = await _context.Profiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null) return null;

        profile.Address = updatedProfile.Address;
        profile.PhoneNumber = updatedProfile.PhoneNumber;
        profile.Gender = updatedProfile.Gender;
        profile.ProfilePicture = updatedProfile.ProfilePicture;

        await _context.SaveChangesAsync();
        return profile;
    }
}