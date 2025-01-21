using CartApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartApi.Application.DTOs.Mappers
{
    public static class CartMapper
    {
        // Convert Cart Entity to CartDto
        public static CartDto ToDto(Cart cart)
        {
            return new CartDto(
                cart.Id,
                cart.CreatedAt,
                cart.TotalAmount,
                cart.UserId,
                cart.CartLines.Select(ToDto).ToList() // Convert CartLine entities to CartLineDto
            );
        }

        // Convert CartDto to Cart Entity
        public static Cart ToEntity(CartDto cartDto)
        {
            return new Cart
            {
                Id = cartDto.Id,
                CreatedAt = cartDto.CreatedAt,
                TotalAmount = cartDto.TotalAmount,
                UserId = cartDto.UserId,
                CartLines = cartDto.CartLines.Select(ToEntity).ToList()
            };
        }

        // Convert CartLine Entity to CartLineDto
        public static CartLineDto ToDto(CartLine cartLine)
        {
            return new CartLineDto(
                cartLine.Id,
                cartLine.Quantity,
                cartLine.SubTotal,
                cartLine.ProductId
            );
        }

        // Convert CartLineDto to CartLine Entity
        public static CartLine ToEntity(CartLineDto cartLineDto)
        {
            return new CartLine
            {
                Id = cartLineDto.Id,
                Quantity = cartLineDto.Quantity,
                SubTotal = cartLineDto.SubTotal,
                ProductId = cartLineDto.ProductId
            };
        }
    }
}
