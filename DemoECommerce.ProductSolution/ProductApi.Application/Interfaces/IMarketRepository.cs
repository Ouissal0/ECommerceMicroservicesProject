using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;

namespace ProductApi.Domain.Interfaces
{
    public interface IMarketRepository
    {
        Task<IEnumerable<Market>> GetAllAsync();
        Task<Market> FindByIdAsync(long id);
        Task<eCommerce.SharedLibrary.Response.Response> CreateAsync(Market market);
        Task<eCommerce.SharedLibrary.Response.Response> UpdateAsync(Market market);
        Task<eCommerce.SharedLibrary.Response.Response> DeleteAsync(long id);
       
    }
}
