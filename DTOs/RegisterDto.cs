using System.ComponentModel.DataAnnotations;

namespace GlamoraApi.DTOs
{
    public class RegisterDto
    {
        public required string Name { get; set; }
     
        public required string Email { get; set; }
       
        public required string PasswordHash { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
