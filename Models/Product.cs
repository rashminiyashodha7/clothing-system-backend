using GlamoraApi.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlamoraApi.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductQuantity { get; set; }
        public decimal ProductDiscount { get; set; }
        public DateTime ProductCreatedDate { get; set; }
        [NotMapped]
        public IFormFile? ProductImage { get; set; }
        public string ProductImageUrl { get; set; }
        public Category? Category { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
