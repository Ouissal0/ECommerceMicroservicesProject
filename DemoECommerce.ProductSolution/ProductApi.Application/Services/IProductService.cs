using ProductApi.Domain.Entities;


namespace ProductApi.Application.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(long id);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(long categoryId);
        Task AddAsync(Product product);
       // Task UpdateAsync(Product product);
        Task DeleteAsync(long id);

        Task<IEnumerable<Product>> GetProductsByMarketAsync(long marketId);

    }

}
