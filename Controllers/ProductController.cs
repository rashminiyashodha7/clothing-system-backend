using GlamoraApi.Core.Interfaces;
using GlamoraApi.DTOs;
using GlamoraApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlamoraApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return NotFound($"Product with ID {id} not found.");
            return Ok(product);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetProductByName(string name)
        {
            var product = await _productService.GetProductByName(name);
            if (product == null)
                return NotFound($"Product with name '{name}' not found.");
            return Ok(product);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategory(categoryId);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDto productDto)
        {
            if (productDto is null)
                return BadRequest("Invalid product data.");

            var createdProduct = await _productService.CreateProduct(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.ProductId }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDto productDto)
        {
            if (productDto == null || id != productDto.ProductId)
                return BadRequest("Invalid product data.");

            var updatedProduct = await _productService.UpdateProduct(id, productDto);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deletedProduct = await _productService.DeleteProduct(id);
            if (deletedProduct == null)
                return NotFound($"Product with ID {id} not found.");
            return Ok(deletedProduct);
        }

        [HttpPut("discount/{id}/{discount}")]
        public async Task<IActionResult> ApplyDiscount(int id, decimal discount)
        {
            var product = await _productService.ApplyDiscount(id, discount);
            if (product == null)
                return NotFound($"Product with ID {id} not found.");
            return Ok(product);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string name, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] int? categoryId)
        {
            var products = await _productService.SearchProducts(name, minPrice, maxPrice, categoryId);
            return Ok(products);
        }

        [HttpGet("new-women")]
        public async Task<IActionResult> GetNewWomenProducts()
        {
            var products = await _productService.GetNewWomenProducts(1);
            return Ok(products);
        }

        [HttpGet("most-recent/category/{categoryId}")]
        public async Task<IActionResult> GetMostRecentProductsByCategory(int categoryId, [FromQuery] int limit = 3)
        {
            var products = await _productService.GetMostRecentProductsByCategory(categoryId, limit);
            return Ok(products);
        }

        [HttpPut("update-quantity/{productId}")]
        public async Task<IActionResult> UpdateProductQuantity(int productId, [FromBody] int quantityChange)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProductQuantity(productId, quantityChange);
                return Ok(new
                {
                    Success = true,
                    ProductId = updatedProduct.ProductId,
                    NewQuantity = updatedProduct.ProductQuantity,
                    Message = "Quantity updated successfully"
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Internal server error",
                    Detail = ex.Message
                });
            }
        }

        [HttpPut("restore-quantity/{productId}")]
        public async Task<IActionResult> RestoreProductQuantity(int productId, [FromBody] int quantity)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProductQuantity(productId, quantity);
                return Ok(new
                {
                    Success = true,
                    ProductId = updatedProduct.ProductId,
                    NewQuantity = updatedProduct.ProductQuantity,
                    Message = "Quantity restored successfully"
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Internal server error",
                    Detail = ex.Message
                });
            }
        }

    }
}
