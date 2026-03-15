using GlamoraApi.Models;

namespace GlamoraApi.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task<Product> GetProductByName(string name);
        Task<List<Product>> GetProductsByCategory(int categoryId);
        Task<List<Product>> GetMostRecentProductsByCategory(int categoryId, int count);

        // CRUD operations

        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<Product> DeleteProduct(int id);

        
   
    }
}
