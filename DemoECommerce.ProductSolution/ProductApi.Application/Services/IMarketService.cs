using eCommerce.SharedLibrary.Response;
using ProductApi.Application.DTOs;

namespace Application.Services
{
    public interface IMarketService
    {
        Task<Response> AddMarketAsync(MarketDto marketDto);
        Task<Response> UpdateMarketAsync(MarketDto marketDto);
        Task<Response> DeleteMarketAsync(long id);
        Task<IEnumerable<MarketDto>> GetAllMarketsAsync();
        Task<MarketDto?> GetMarketByIdAsync(long id);
        Task<bool> DoesOwnerExistAsync(string owner);
        Task<long> GetMarketByOwnerAsync(string owner);
    }
}
