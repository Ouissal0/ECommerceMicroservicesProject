using CartApi.Application.Interfaces;
using CartApi.Domain.Entities;
using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Response;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;

namespace CartApi.Infrastructure.Repositories
{
    public class CartRepository : ICart
    {


        private readonly IDatabase _redisDb;

        public CartRepository(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }
        private string GetRedisKey(int cartId) => $"cart:{cartId}";
        private string GetUserKey(long userId) => $"user:{userId}:cart";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromHours(1);


        private async Task<Cart?> GetFromCacheAsync(int cartId)
        {
            var cachedValue = await _redisDb.StringGetAsync(GetRedisKey(cartId));
            return cachedValue.HasValue ? JsonSerializer.Deserialize<Cart>(cachedValue!) : null;
        }

        private async Task SetCacheAsync(Cart cart)
        {
            var serialized = JsonSerializer.Serialize(cart);
            await _redisDb.StringSetAsync(GetRedisKey(cart.Id), serialized, _cacheExpiration);
        }

        private async Task RemoveCacheAsync(int cartId)
        {
            await _redisDb.KeyDeleteAsync(GetRedisKey(cartId));
        }
        public async Task<Cart> AddAsync(Cart cartEntity)
        {
            if (cartEntity == null)
            {
                throw new ArgumentNullException(nameof(cartEntity), "The cart entity cannot be null.");
            }

            try
            {
                // Ajouter le panier dans Redis
                await SetCacheAsync(cartEntity);
                return cartEntity;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new ApplicationException($"Error occurred while adding the cart with ID {cartEntity.Id}.", ex);
            }
        }
        public async Task<Cart?> GetCartAsync(int cartId)
        {
            // Utilisez la même clé de Redis que celle utilisée pour ajouter un panier
            var cartData = await _redisDb.StringGetAsync($"cart:{cartId}");
            if (string.IsNullOrEmpty(cartData)) return null;

            return JsonSerializer.Deserialize<Cart>(cartData);
        }


        public async Task UpdateCartAsync(Cart cart)
        {
            // S'assurer que chaque CartLine a son sous-total recalculé avant de sauvegarder
            foreach (var line in cart.CartLines)
            {
                line.CalculateSubTotal(line.ProductId);  // Vous aurez peut-être besoin d'un prix unitaire ici
            }

            var cartData = JsonSerializer.Serialize(cart);
            await _redisDb.StringSetAsync($"cart:{cart.Id}", cartData); // Utilisez la bonne clé Redis ici
        }

        public async Task DeleteCartAsync(int cartId)
        {
            await _redisDb.KeyDeleteAsync($"cart:{cartId}");
        }



    }
}
