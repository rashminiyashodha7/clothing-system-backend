namespace GlamoraApi.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int ProductId { get; set; } //foreign key to Product
        public Product Product { get; set; } //navigation property
        public int CartId { get; set; } //foreign key to Cart
        public Cart Cart { get; set; } //navigation property
        
        

    }
}
