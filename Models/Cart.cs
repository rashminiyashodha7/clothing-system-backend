namespace GlamoraApi.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        public int UserId { get; set; } //foreign key to User
        public User User { get; set; } //navigation property
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
