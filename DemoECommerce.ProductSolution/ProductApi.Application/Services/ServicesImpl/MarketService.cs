using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Response;
using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interfaces;

namespace Application.Services.ServicesImpl
{ 

    public class MarketService : IMarketService
    {
        private readonly IMarketRepository _marketRepository;

        public MarketService(IMarketRepository marketRepository)
        {
            _marketRepository = marketRepository;
        }

        public async Task<IEnumerable<MarketDto>> GetAllMarketsAsync()
        {
            var markets = await _marketRepository.GetAllAsync();
            return markets.Select(m => new MarketDto
            {
                Id = m.Id,
                Name = m.Name,
                Owner=m.Owner,
                Description = m.Description,
                Longitude = m.Longitude,
                Latitude = m.Latitude,
                Image = m.Image,
                PhoneNumber = m.PhoneNumber

            });
        }


        public async Task<MarketDto?> GetMarketByIdAsync(long id)
        {
            var m = await _marketRepository.FindByIdAsync(id);
            return m == null ? null : new MarketDto
            {
                Id = m.Id,
                Name = m.Name,
                Owner = m.Owner,
                Description = m.Description,
                Longitude = m.Longitude,
                Latitude = m.Latitude,
                Image = m.Image,
                PhoneNumber = m.PhoneNumber
            };
        }




        public async Task<Response> AddMarketAsync(MarketDto marketDto)
        {
            try
            {
                var market = new Market
                {
                    Name = marketDto.Name,
                    Owner = marketDto.Owner,
                    Description = marketDto.Description,
                    Longitude = marketDto.Longitude,
                    Latitude = marketDto.Latitude,
                    Image = marketDto.Image,
                    PhoneNumber = marketDto.PhoneNumber

                };

                // Insérez le marché dans la base de données
                await _marketRepository.CreateAsync(market);

                // Assignez l'Id généré au DTO
                marketDto.Id = market.Id;

                // Retournez une réponse avec l'Id généré
                return new Response(true, "Market added successfully.");
            }
            catch (Exception ex)
            {
                // Log l'exception
                LogException.LogExceptions(ex);
                return new Response(false, "Failed to add market.");
            }
        }


        public async Task<Response> UpdateMarketAsync(MarketDto marketDto)
            {
                try
                {
                    var market = await _marketRepository.FindByIdAsync(marketDto.Id);
                    if (market == null)
                    {
                        return new Response(false, "Market not found.");
                    }

                    market.Name = marketDto.Name;
                    market.Description = marketDto.Description;
                    market.Longitude = marketDto.Longitude;
                    market.Latitude = marketDto.Latitude;
                    market.Image = marketDto.Image;

                    await _marketRepository.UpdateAsync(market);
                    return new Response(true, "Market updated successfully.");
                }
                catch (Exception ex)
                {
                    LogException.LogExceptions(ex);
                    return new Response(false, "Failed to update market.");
                }
            }

            public async Task<Response> DeleteMarketAsync(long id)
            {
                try
                {
                    var market = await _marketRepository.FindByIdAsync(id);
                    if (market == null)
                    {
                        return new Response(false, "Market not found.");
                    }

                    await _marketRepository.DeleteAsync(id);
                    return new Response(true, "Market deleted successfully.");
                }
                catch (Exception ex)
                {
                    LogException.LogExceptions(ex);
                    return new Response(false, "Failed to delete market.");
                }
            }
        public async Task<bool> DoesOwnerExistAsync(string owner)
        {
            var exists = await _marketRepository.GetAllAsync();
            return exists.Any(m => m.Owner == owner);
        }

        public async Task<long> GetMarketByOwnerAsync(string owner)
        {
            try
            {
                // Rechercher les marchés par propriétaire
                var markets = await _marketRepository.GetAllAsync();
                var market = markets.FirstOrDefault(m => m.Owner == owner);

                if (market == null)
                {
                    throw new Exception("Market with the specified owner does not exist.");
                }

                return market.Id;
            }
            catch (Exception ex)
            {
                // Log l'exception
                LogException.LogExceptions(ex);
                throw new Exception("An error occurred while retrieving the market by owner.", ex);
            }
        }
    }
}

