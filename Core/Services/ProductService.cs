using GlamoraApi.Core.Enums;
using GlamoraApi.Core.Interfaces;
using GlamoraApi.Data.Repositories;
using GlamoraApi.DTOs;
using GlamoraApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;

namespace GlamoraApi.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public readonly IWebHostEnvironment _hostingEnvironment;

        public ProductService(IProductRepository repository, IWebHostEnvironment hostingEnvironment)
        {
            _productRepository = repository;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<Product> CreateProduct(ProductDto productDto)
        {
            if (string.IsNullOrWhiteSpace(productDto.ProductName))
            {
                throw new ArgumentException("Product name cannot be empty.");
            }

            var product = new Product
            {

                ProductName = productDto.ProductName,
                ProductDescription = productDto.ProductDescription,
                ProductPrice = productDto.ProductPrice,
                ProductQuantity = productDto.ProductQuantity,
                Category = productDto.Category,
            };

            if (productDto.ProductImage != null)
            {
                product.ProductImageUrl = await SaveImage(productDto.ProductImage);
            }

            return await _productRepository.CreateProduct(product);
        }

        public async Task<Product> DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product is null)
            {
                throw new ArgumentException("Product not found.");
            }

            await _productRepository.DeleteProduct(id);
            return product;
        }

        public async Task<Product> UpdateProduct(int id, ProductDto productDto)
        {
            if (string.IsNullOrWhiteSpace(productDto.ProductName))
            {
                throw new ArgumentException("Product name cannot be empty.");
            }

            var product = await _productRepository.GetProductById(id);
            if (product is null)
            {
                throw new ArgumentException("Product not found.");
            }

            product.ProductName = productDto.ProductName;
            product.ProductDescription = productDto.ProductDescription;
            product.ProductPrice = productDto.ProductPrice;
            product.ProductQuantity = productDto.ProductQuantity;
            product.ProductImage = productDto.ProductImage;
            product.Category = productDto.Category;

            if (productDto.ProductImage != null)
            {
                product.ProductImageUrl = await SaveImage(productDto.ProductImage);
            }

            return await _productRepository.UpdateProduct(product);
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<Product> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);
            return product;
        }

        public async Task<Product> GetProductByName(string name)
        {
            var product = await _productRepository.GetProductByName(name);
            return product;
        }

        public async Task<List<Product>> GetProductsByCategory(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategory(categoryId);
            return products;
        }

        //other operations
        public async Task<Product> ApplyDiscount(int id, decimal discount)
        {
            var product = await _productRepository.GetProductById(id);
            if (product is null)
            {
                throw new ArgumentException("Product not found.");
            }
            product.ProductPrice = product.ProductPrice - discount;
            return await _productRepository.UpdateProduct(product);
        }

        public async Task<List<Product>> SearchProducts(string keyword, decimal? minPrice, decimal? maxPrice, int? categoryId)
        {
            var allProducts = await _productRepository.GetAllProducts();

            if (!string.IsNullOrWhiteSpace(keyword))
                allProducts = allProducts.Where(p => p.ProductName.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

            if (minPrice.HasValue)
                allProducts = allProducts.Where(p => p.ProductPrice >= minPrice.Value).ToList();

            if (maxPrice.HasValue)
                allProducts = allProducts.Where(p => p.ProductPrice <= maxPrice.Value).ToList();

           

            return allProducts;
        }


        public async Task<List<Product>> GetNewWomenProducts(int categoryId)
        {
            var tenDays = DateTime.UtcNow.AddDays(-10);
            var products = await _productRepository.GetProductsByCategory(categoryId);
            return products.Where(products => products.ProductCreatedDate >= tenDays).ToList();

        }

        public async Task<List<Product>> GetMostRecentProductsByCategory(int categoryId, int count = 10)
        {
            var products = await _productRepository.GetProductsByCategory(categoryId);
            return products.OrderByDescending(p => p.ProductCreatedDate).Take(count).ToList();
        }

        public async Task<Product> UpdateProductQuantity(int productId, int quantityChange)
        {
            if (productId <= 0)
            {
                throw new ArgumentException("Invalid product ID");
            }

            var product = await _productRepository.GetProductById(productId);
            if (product is null)
            {
                throw new ArgumentException($"Product with ID {productId} not found");
            }

            // Ensure quantity doesn't go negative
            var newQuantity = product.ProductQuantity + quantityChange;
            if (newQuantity < 0)
            {
                throw new InvalidOperationException(
                    $"Cannot update quantity. Result would be negative (Current: {product.ProductQuantity}, Change: {quantityChange})");
            }

            product.ProductQuantity = newQuantity;
            return await _productRepository.UpdateProduct(product);
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return Path.Combine("images", uniqueFileName);
        }

    }
}
