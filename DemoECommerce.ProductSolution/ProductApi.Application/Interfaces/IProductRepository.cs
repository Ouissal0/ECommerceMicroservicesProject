using ProductApi.Domain.Entities;


namespace ProductApi.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(long id);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(long categoryId);
        Task<eCommerce.SharedLibrary.Response.Response> AddAsync(Product product);
      //  Task<eCommerce.SharedLibrary.Response.Response> UpdateAsync(Product product);
        Task<eCommerce.SharedLibrary.Response.Response> DeleteAsync(long id);
        Task<IEnumerable<Product>> GetByMarketIdAsync(long marketId);
    }

}

