using CartApi.Application.Interfaces;
using CartApi.Application.Services;
using CartApi.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using eCommerce.SharedLibrary.Logs;
using CartApi.Domain.Entities;

namespace CartApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        
            private readonly ICartService _cartService;

            public CartController(ICartService cartService)
            {
                _cartService = cartService;
            }
        [HttpGet]
        public IActionResult TestApi()
        {
            return Ok("API is running");
        }
        // Action pour ajouter un panier
        [HttpPost]
        public async Task<IActionResult> AddCart([FromBody] Cart cart)
        {
            try
            {
                if (cart == null)
                    return BadRequest("Cart data is required.");

                var createdCart = await _cartService.AddCartAsync(cart);

                return CreatedAtAction(nameof(GetCart), new { cartId = createdCart.Id }, createdCart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while adding the cart.", Details = ex.Message });
            }
        }

        [HttpGet("{cartId}")]
            public async Task<IActionResult> GetCart(int cartId)
            {
                var cart = await _cartService.GetCartAsync(cartId);
                if (cart == null) return NotFound("Cart not found");

                return Ok(cart);
            }

        [HttpPut("{cartId}")]
        public async Task<IActionResult> UpdateCart(int cartId, [FromBody] List<CartLine> updatedCartLines)
        {
            if (updatedCartLines == null || !updatedCartLines.Any())
            {
                return BadRequest("Updated cart lines cannot be empty.");
            }

            try
            {
                await _cartService.UpdateCartAsync(cartId, updatedCartLines);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{cartId}")]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            try
            {
                await _cartService.DeleteCartAsync(cartId);
                return NoContent(); // Retourne 204 si suppression réussie
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Retourne 404 si le panier n'existe pas
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Retourne 500 en cas d'erreur inattendue
            }
        }


    }

}
