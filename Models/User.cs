using System.ComponentModel.DataAnnotations;

namespace GlamoraApi.Models
{
    public class User
    {
        public int UserId { get; set; }
        public  string Name { get; set; }
        public  string Email { get; set; }
        public  string PasswordHash { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Profile Profile { get; set; } 
        public Cart Cart { get; set; }

    }
}
