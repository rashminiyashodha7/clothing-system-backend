using GlamoraApi.Core.Enums;
using GlamoraApi.Core.Interfaces;
using GlamoraApi.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GlamoraApi.Data.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var currentProduct = await _context.Products.FindAsync(product.ProductId);
            if (currentProduct is not null)
            {
                product.ProductId = currentProduct.ProductId;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
            return product;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }

        public async Task<Product> GetProductById(int id)
        {
            var productById = await _context.Products.FindAsync(id);
            return productById;
        }

        public async Task<Product> GetProductByName(string name)
        {
            var productByname = await _context.Products.FirstOrDefaultAsync(p => p.ProductName == name);
            return productByname;

        }

        public async Task<List<Product>> GetProductsByCategory(int categoryId)
        {
           
            Category category = (Category)categoryId;

            var productsByCategory = await _context.Products
                .Where(p => p.Category == category)  
                .ToListAsync();

            return productsByCategory;
        }

        public async Task<List<Product>> GetMostRecentProductsByCategory(int categoryId, int count)
        {
            Category category = (Category)categoryId;
            var productsByCategory = await _context.Products
                .Where(p => p.Category == category)
                .OrderByDescending(p => p.ProductCreatedDate)
                .Take(count)
                .ToListAsync();
            return productsByCategory;
        }

    }
}
