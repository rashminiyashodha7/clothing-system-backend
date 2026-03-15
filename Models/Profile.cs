using GlamoraApi.Core.Enums;

namespace GlamoraApi.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public string? ProfilePicture { get; set; }
        public int UserId { get; set; } //foreign key to User
        public User User { get; set; } //navigation property

    }
}
