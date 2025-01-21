using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interfaces;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("Market/{marketId}")]
        public async Task<IActionResult> GetAllProductsByMarket(long marketId)
        {
            var products = await _productRepository.GetByMarketIdAsync(marketId);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(long id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }
     
        [HttpGet("Category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategoryId(long categoryId)
        {
            var products = await _productRepository.GetByCategoryIdAsync(categoryId);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            var response = await _productRepository.AddAsync(product);

            if (!response.Flag)
                return BadRequest(response.Message);

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { Message = "Product not found" });
            }

            var response = await _productRepository.DeleteAsync(id);
            if (!response.Flag)
            {
                return BadRequest(new { Message = response.Message });
            }

            return Ok(new { Message = "Product deleted successfully" });
        }

    }
}
