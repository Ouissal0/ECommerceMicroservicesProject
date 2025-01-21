using CartApi.Application.DTOs;
using CartApi.Application.Interfaces;
using CartApi.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CartApi.Application.Services
{
    public interface ICartService
    {

        Task<Cart> AddCartAsync(Cart cart);

        Task<Cart?> GetCartAsync(int cartId);
        Task UpdateCartAsync(int cartId, List<CartLine> updatedCartLines);
          Task DeleteCartAsync(int cartId);

    }
}
