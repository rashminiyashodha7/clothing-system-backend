using GlamoraApi.DTOs;
using GlamoraApi.Models;

namespace GlamoraApi.Core.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task<Product> GetProductByName(string name);
        Task<List<Product>> GetProductsByCategory(int categoryId);

        // CRUD operations
        Task<Product> CreateProduct(ProductDto productDto);
        Task<Product> UpdateProduct(int id, ProductDto productDto);
        Task<Product> DeleteProduct(int id);

        // Other operations

        Task<Product> ApplyDiscount(int id, decimal discount);
        Task<List<Product>> SearchProducts(string name, decimal? minPrice, decimal? maxPrice, int? categoryId);
        Task<List<Product>> GetNewWomenProducts(int categoryId);
        Task<List<Product>> GetMostRecentProductsByCategory(int categoryId, int count=10);
        Task<Product> UpdateProductQuantity(int productId, int quantityChange);

    }
}
