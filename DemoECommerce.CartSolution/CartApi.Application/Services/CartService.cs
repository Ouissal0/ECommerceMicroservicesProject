using CartApi.Application.DTOs;
using CartApi.Application.Interfaces;
using CartApi.Domain.Entities;
using eCommerce.SharedLibrary.Logs;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Registry;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CartApi.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICart _cartRepository;

        public CartService(ICart cartRepository)
        {

            _cartRepository = cartRepository;
           
        }

        public async Task<Cart?> GetCartAsync(int cartId)
        {
            return await _cartRepository.GetCartAsync(cartId);
        }

        public async Task<Cart> AddCartAsync(Cart cart)
        {
            // Validation de base
            if (cart == null)
                throw new ArgumentNullException(nameof(cart), "Cart cannot be null.");

            if (cart.UserId <= 0)
                throw new ArgumentException("Invalid UserId.");

            if (cart.CartLines == null || cart.CartLines.Count == 0)
                throw new ArgumentException("Cart must have at least one cart line.");

            // Calcul du total
            cart.CalculateTotal();

            // Ajout au repository
            return await _cartRepository.AddAsync(cart);
        }
        public async Task UpdateCartAsync(int cartId, List<CartLine> updatedCartLines)
        {
            // Récupérer le panier existant
            var cart = await _cartRepository.GetCartAsync(cartId);
            if (cart == null)
                throw new KeyNotFoundException("Cart not found");

            // Remplacer les lignes existantes par celles mises à jour
            cart.CartLines = updatedCartLines;

            // Recalculer le montant total
            cart.CalculateTotal();

            // Sauvegarder les modifications
            await _cartRepository.UpdateCartAsync(cart);
        }

        public async Task DeleteCartAsync(int cartId)
        {
            var cart = await _cartRepository.GetCartAsync(cartId);
            if (cart == null)
            {
                throw new KeyNotFoundException($"Cart with ID {cartId} not found.");
            }

            await _cartRepository.DeleteCartAsync(cartId);
        }

        /*  public async Task UpdateCartAsync(int cartId, List<CartLine> updatedCartLines)
          {
              var cart = await _cartRepository.GetCartAsync(cartId);
              if (cart == null)
                  throw new KeyNotFoundException("Cart not found");

              // Assurez-vous que les CartLines sont valides
              if (updatedCartLines == null || !updatedCartLines.Any())
                  throw new ArgumentException("Cart lines cannot be empty.");

              // Mettez à jour les CartLines et calculez les sous-totaux
              foreach (var line in updatedCartLines)
              {
                  // Trouver la ligne correspondante dans le panier
                  var existingLine = cart.CartLines.FirstOrDefault(c => c.Id == line.Id);
                  if (existingLine != null)
                  {
                      // Mettez à jour la quantité et le sous-total de cette ligne
                      existingLine.ModifyQuantity(line.Quantity, existingLine.ProductId); // Assurez-vous que vous avez un prix d'unité
                  }
                  else
                  {
                      // Si la ligne n'existe pas, vous pouvez la rajouter
                      cart.CartLines.Add(line);
                  }
              }

              // Recalculez le total
              cart.CalculateTotal();

              // Mettez à jour le panier dans Redis
              await _cartRepository.UpdateCartAsync(cart);
          }

          */



    }
}
