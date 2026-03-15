using GlamoraApi.Core.Enums;

namespace GlamoraApi.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductDiscount { get; set; }
        public DateTime ProductCreatedDate { get; set; }
        public int ProductQuantity { get; set; }
        public IFormFile? ProductImage { get; set; }
        public Category Category { get; set; }

    }
}
