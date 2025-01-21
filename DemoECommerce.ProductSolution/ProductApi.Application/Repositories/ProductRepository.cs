using eCommerce.SharedLibrary.Logs;
using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MarketDbContext _context;

        public ProductRepository(MarketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(long id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(long categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<eCommerce.SharedLibrary.Response.Response> AddAsync(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return new eCommerce.SharedLibrary.Response.Response(true, "Product added successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new eCommerce.SharedLibrary.Response.Response(false, "An error occurred while adding the product.");
            }
        }


     /*   public async Task<eCommerce.SharedLibrary.Response.Response> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }*/

        public async Task<eCommerce.SharedLibrary.Response.Response> DeleteAsync(long id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return new eCommerce.SharedLibrary.Response.Response(false, "Product not found.");

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return new eCommerce.SharedLibrary.Response.Response(true, "Product deleted successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new eCommerce.SharedLibrary.Response.Response(false, "An error occurred while deleting the product.");
            }
        }

        public async Task<IEnumerable<Product>> GetByMarketIdAsync(long marketId)
        {
            return await _context.Products
                .Where(p => p.MarketId == marketId)
                .ToListAsync();
        }

    }
}
